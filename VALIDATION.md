# Validation

How request validation works in BeHealthy, where each kind of rule lives, and how to add one.

## The three kinds of rules

| Kind | Examples | Needs | Lives in | Runs on |
|---|---|---|---|---|
| **Shape** | required, max length, email format, `End > Start`, enum in range, page size 1–100 | only the request | `src/BeHealthy.Validation` | Front **and** API |
| **Setting-driven** | "a doctor needs a specialty" (`DoNotAllowDoctorWithoutSpecialty`), "an appointment needs a room" | an app setting | `src/BeHealthy.Validation`, read through `IValidationSettingsProvider` | Front **and** API |
| **Database** | the patient exists, the email is not used yet, a setting's value fits its type | the database | `src/BeHealthy.Application/Validators` (`*ServerValidator`) | API only |

Rules that depend on the state of the database *at the moment of writing* (for example overlapping appointments) are not validation. They stay in the service, as part of the write.

The API is the source of truth. The Front runs the same shape and setting rules first, so users get instant feedback. Anything only the server can check comes back as a 400 and is shown on the form.

## How a request is validated on the API

```
HTTP request
  → [ApiController] model binding (malformed JSON, wrong types → 400)
  → ValidationFilter                    src/BeHealthy.API/Filters/ValidationFilter.cs
       for each action argument (body or [FromQuery] object):
         run every IValidator<T> registered for its type
           - the shared validator        e.g. DoctorCreateRequestValidator
           - the server validator        e.g. DoctorCreateRequestServerValidator
       any failures → 400 ValidationProblemDetails, the action never runs
  → controller → service
```

The filter is registered globally in `Program.cs`, so a new endpoint is validated as soon as its request type has a validator. `GlobalExceptionHandler` also turns a thrown `FluentValidation.ValidationException` into the same 400, as a safety net.

A failed request looks like this:

```json
{
  "status": 400,
  "title": "One or more validation errors occurred.",
  "errors": {
    "Email": ["Email is already used"],
    "SpecialtyId": ["Specialty is required"]
  }
}
```

The keys are the request's property names.

## Setting-driven rules (no constructor flags)

Validators never take a `bool` in their constructor. A rule that depends on an admin setting asks `IValidationSettingsProvider`:

```csharp
public DoctorCreateRequestValidator(IValidationSettingsProvider settings)
{
    WhenAsync((_, ct) => settings.IsEnabledAsync(AppSettingKeys.DoNotAllowDoctorWithoutSpecialty, ct), () =>
    {
        RuleFor(x => x.SpecialtyId).NotNull().WithMessage(ValidationMessages.Required(Resource.Specialty));
    });
}
```

Each host registers its own implementation, so the same validator class works in both places:

| Host | Implementation | Reads from |
|---|---|---|
| API | `BeHealthy.Application.Validators.ValidationSettingsProvider` | the AppSettings table |
| Front | `BeHealthy.Front.Services.Validation.ValidationSettingsProvider` | the settings API |

Validators are registered as **scoped** services, so always resolve them from DI. Never `new` them. Rules like `WhenAsync` and `MustAsync` are async, so call `ValidateAsync`, never `Validate`.

## How the Front uses the validators

`AddSharedValidators()` in `src/BeHealthy.Front/DependencyInjection.cs` registers them. Forms use one of three styles, depending on what the form is bound to:

1. **Blazored `<FluentValidationValidator />`**, when the form is bound to the request DTO itself (nurse and patient create/edit). Blazored finds the validator in DI on its own.
2. **Injected `IValidator<TRequest>` + `<ValidationComponent>`**, when the form is bound to a *response* DTO (appointment, room, specialty, visit) or validates by hand (doctor create/edit). Validate the request you are about to send (for example `appointmentDto.MapToCreationDto()`) and pass `result.GetErrorsGroupedByProperty()` to `ValidationComponent.DisplayErrors`. The property names match, so the messages land on the right inputs.
3. **Custom validation**, kept on purpose as examples: DataAnnotations in `PrescriptionModal` and hand-written checks in `Department/GeneralData`. The API still validates those requests with FluentValidation, and the forms show the API's message when it rejects the save.

When the API rejects a save, `ApiClientBase` returns a `ServiceResponse` whose `ValidationErrors` holds the per-field messages, and whose `ErrorMessage` lists them all. Forms with a `ValidationComponent` call `validationComponent?.DisplayErrors(response.ValidationErrors)`. Pages that only toast use `response.ErrorMessage`.

## Conventions

- **One message per field.** The rule chains in `RuleBuilderExtensions` stop at the first failure. Server rules skip values the shared validator already rejects (an id ≤ 0, an empty email), so they don't add a second message.
- **Optional dropdowns send `null`, not `0`.** Render the placeholder as `<option value="">`. `OptionalReference` rejects `0`, and `RequiredReference` treats `0` as "not selected".
- **Lengths come from `FieldLengths`.** The EF configurations use the same constants. SQLite does not enforce `HasMaxLength`, so the validator is the only real guard.
- **The password policy comes from `PasswordPolicy`.** Identity is configured from the same constants in `src/BeHealthy.Infrastructure/DependencyInjection.cs`.
- **Messages come from `ValidationMessages` / `Resource`**, never hard-coded English.
- **Setting keys come from `AppSettingKeys`**, never string literals.

## Adding validation for a new request

1. Create `src/BeHealthy.Validation/<Area>/<Request>Validator.cs` (`AbstractValidator<TRequest>`) with the shape and setting rules. Reuse `RuleBuilderExtensions` (`RequiredText`, `PersonName`, `Email`, `RequiredReference`, ...).
2. If a rule needs the database, add `src/BeHealthy.Application/Validators/<Area>/<Request>ServerValidator.cs` and use `MustExist` / `MustBeUnusedEmail` from `ServerRuleExtensions`.
3. Nothing to register by hand: both assemblies are scanned (`AddSharedValidators()` and `AddValidatorsFromAssemblyContaining<ValidationSettingsProvider>()`).
4. In the Blazor form, use one of the styles above.
5. Add tests under `tests/BeHealthy.Tests/UnitTests/Validation`. `ValidatorRegistrationTests` already fails if a `*Request` DTO or `QueryParameters` type has no validator.
