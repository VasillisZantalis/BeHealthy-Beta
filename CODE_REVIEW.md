# Code Review — Smells, Bad Practices & Improvements

> Written 2026-10-02 against `main` @ `637fdc7`. This adds to [`KNOWN_ISSUES.md`](KNOWN_ISSUES.md) rather than repeating it. Where an item overlaps, it says so.
> Ordered by priority inside each section. **Backend first**, frontend notes at the end.
> Legend: 🔴 bug / data risk · 🟠 design smell with real consequences · 🟡 cleanup / nice to have

---

## 0. Status of `KNOWN_ISSUES.md` items

| # | Item | Status now |
|---|------|-----------|
| 1 | No server-side validation | **Still open.** See §3 for a concrete design. |
| 2 | IDOR / missing authorization | **Partly fixed.** Role attributes (`RoleGroups`) were added, but ownership checks are still missing. Example: a `Patient` can `GET /api/medical-records` and `GET /api/prescriptions` and receive **every patient's** records. See §4. |
| 3 | Login bypasses lockout | Still open (`AuthService.cs:24`). `SignInManager` is already registered in Infrastructure DI, so switching to it is a one-liner. |
| 4 | DB wiped on startup | Still open (`Infrastructure/DependencyInjection.cs:62`). |
| 6 | Singleton `ToastService` | **Fixed.** It is now `Scoped`. Both toast services still exist (see §F). |
| 7 | Fire-and-forget API calls | Still open (`ApiClientBase.PostAsync/PutAsync/DeleteAsync`). |
| 8 | Missing FK checks | Still open. |
| 9 | Double-booking race | Still open. §1.5 covers more conflict-check bugs. |
| 10 | Settings page not gated | Still open. `Settings.razor` has no `[Authorize]`. |

---

## 1. Backend correctness bugs

### 1.1 🔴 Pagination `TotalCount` ignores the filter
`AppointmentService.GetAllAppointmentsAsync`, `NurseService.GetAllNursesAsync` and `DoctorService.GetAllDoctorsAsync` set `TotalCount = repository.GetCountAsync()`, which counts the **whole table**. If you search or filter, the paginator still shows pages for every row.

`DoctorService` is worse. It also runs a second query that loads **every matching doctor** into `allDoctors` (`DoctorService.cs:62`), never uses the result, and then counts the whole table anyway.

**Fix:** add `Task<int> CountAsync(Expression<Func<T,bool>>? predicate)` to the generic repository and pass it the same predicate. Better still, let `QueryAsync` return `(IReadOnlyList<T> Items, int Total)`.

### 1.2 🔴 Creating a prescription can never succeed
`Prescription.TreatmentId` is a **required** `int` FK (`Prescriptions.TreatmentId` is non-nullable in the model snapshot, with cascade delete). `PrescriptionCreateRequest` has no `TreatmentId`, so every insert writes `TreatmentId = 0`. EF Core's SQLite provider enforces foreign keys, so the insert fails. The `catch (Exception)` in `AddPrescriptionAsync` then returns a generic `Failed()` with no logging, so the cause is hidden.

**Fix:** decide on the domain rule. Either make `TreatmentId` nullable (`int?`, `SetNull`), or add it to the request and validate that it exists. Removing the `try/catch` (§2.1) is what would have exposed this bug.

### 1.3 🔴 Conflicting EF relationship configurations
The same relationship is configured in two places with different delete behaviour. The result depends on configuration order:

| Relationship | Config A | Config B | What the snapshot actually has |
|---|---|---|---|
| Visit → Patient | `PatientConfiguration`: **Cascade** | `VisitConfiguration`: **Restrict** | Restrict |
| Room → Department | `DepartmentConfiguration`: **Cascade** | `RoomConfiguration`: **Restrict** | Restrict |
| Nurse → Department | `NurseConfiguration` configures it **twice** | same file | Restrict |
| Doctor → Appointments | `AppointmentConfiguration` | `DoctorConfiguration` (no OnDelete) | Cascade |

Consequences:
- `DELETE /api/patients/{id}` for a patient with visits throws a `DbUpdateException` and returns a 500, because Visit→Patient is Restrict. Meanwhile their appointments, allergies and records **would** cascade.
- Deleting a doctor silently **cascade-deletes all their appointments**, which is clinical history.

**Fix:** configure each relationship once, on the dependent side, and pick delete behaviour on purpose. In a healthcare app, clinical data should almost always be `Restrict` plus soft delete (see §5.9).

### 1.4 🔴 `DateTime` kind conversion relabels instead of converting
`ApplicationUserConfiguration` (DateOfBirth) and `PrescriptionConfiguration` (DatePrescribed) do this:
```csharp
v => DateTime.SpecifyKind(v, DateTimeKind.Utc),    // write
v => DateTime.SpecifyKind(v, DateTimeKind.Local)   // read
```
`SpecifyKind` only changes the label, not the value. A local time is stored as if it were UTC. On read, a UTC value is labelled Local. The stored instant is shifted by the server's UTC offset.

**Fix:** store UTC and read UTC (`v.ToUniversalTime()` on write, `SpecifyKind(v, Utc)` on read). Convert to local time only in the UI. For date-only values such as DateOfBirth, use `DateOnly`.

### 1.5 🔴 Appointment conflict check: several bugs (`AppointmentService.CheckForConflictingAppointmentsAsync`)
- **Back-to-back slots conflict.** `newStart <= existingEnd && newEnd >= existingStart` treats 10:00–11:00 and 11:00–12:00 as overlapping. Use `<` and `>`.
- **Cancelled appointments still block the slot.** None of the four checks (doctor, patient, nurse, room) filter out `Status == Cancelled`.
- **It loads every appointment the doctor or patient has ever had**, with four `Include`s, and filters in memory. Query the database by `AppointmentDate == date` plus the overlap condition, and only return `Any()` or the first match.
- **No server-side check that end > start.** Even the UI validator uses `GreaterThanOrEqualTo`, so a zero-length appointment is accepted.
- **No `NurseId` existence check**, unlike doctor, patient and room. A bad nurse id becomes an FK exception, which is then swallowed (§2.1).
- The race condition is already in KNOWN_ISSUES #9.

### 1.6 🔴 Updating a doctor, nurse or patient trusts two independent ids
`UpdateDoctorAsync`, `UpdateNurseAsync` and `UpdatePatientAsync` load the user by `dto.UserId` and the entity by `dto.Id`, but **never check that `doctor.UserId == dto.UserId`**. A request with `Id = 5` and `UserId = <someone else>` renames doctor 5 and changes another account's name and phone number.

**Fix:** drop `UserId` from the update DTO. Load the entity with `Include(x => x.User)` and update `doctor.User`.

### 1.7 🔴 Not-found handling is inconsistent, and some of it silently succeeds
- `DepartmentRepository.GetDepartmentByIdAsync` uses `FirstAsync`, which **throws** when the id doesn't exist. The result is a 500. The controller's `is null ? NotFoundProblem(...)` branch can never run, and the interface returns a non-nullable `Department`.
- `DeleteAppointment`, `DeleteRoom`, `DeleteSpecialty`, `DeleteMedicalRecord`, `DeleteDoctor`, `DeleteNurse` and `DeletePatient` return **204 even when nothing was deleted**. Allergy, Visit and Prescription correctly report failure.
- `UpdateMedicalRecordNotesAsync` returns silently when the record doesn't exist, so the client gets 204.
- Every failure is turned into **400** by `ProblemFromServiceResponse`, including not-found, so the client can't tell the cases apart. See §2.2.

### 1.8 🟠 Upcoming appointments have no doctor or patient
`GetUpcomingAppointmentsAsync` builds a `QueryOptions` with no `Includes`. `AppointmentMapper` maps `entity.Patient?.MapToDto()`, so the dashboard widget gets `null` doctor and patient objects. It also uses `DateTime.Now` while the rest of the code uses `UtcNow`.

### 1.9 🟠 Audit fields are set by the client
- `MedicalRecordCreateRequest.CreatedBy` and `MedicalRecordUpdateRequest.CreatedBy` come from the request body, so anyone can claim anyone wrote a record. `CreatedUserId` is never set. Both should come from `User.FindFirstValue(ClaimTypes.NameIdentifier)` on the server.
- `AllergyUpdate`, `MedicalRecordUpdate` and `VisitUpdate` let the client **move the record to another patient** (`PatientId = dto.PatientId`). An update should not be able to re-parent a clinical record.
- `Room.CreatedAt` and `Specialty.CreatedAt` are **never set**, so they hold `0001-01-01`. Doctors, nurses and patients set it in the mapper, and `Department` uses a property initializer. Three different conventions for one concern; see §5.8.

### 1.10 🟠 Patients endpoint is unbounded and ignores its own parameters
`GET /api/patients` returns `IEnumerable` with no paging, unlike doctors and nurses. `PatientQueryParameters.FirstName`, `LastName`, `OrderBy`, `PageNumber` and `PageSize` are accepted and sent by the frontend, but **ignored**.

### 1.11 🟠 Paging input isn't clamped
`PageNumber = 0` gives a negative `Skip`, which throws and returns a 500. `PageSize = 0` makes `PaginatedResult.TotalPages` divide by zero. `PageSize = 1_000_000` dumps the whole table. Clamp these in validation (§3) or in `QueryParameters`, for example `PageSize` between 1 and 100.

### 1.12 🟠 `OrderBy` accepts any property path from the client
`OrderByHelper` builds `x => x.<anything>` from a query-string value. `?OrderBy=User.PasswordHash` or `User.SecurityStamp` gives an attacker a sorting oracle over sensitive columns. **Allow-list** the sortable columns per entity:
```csharp
private static readonly Dictionary<string, Expression<Func<Doctor, object>>> Sorts = new(StringComparer.OrdinalIgnoreCase)
{
    ["firstName"] = d => d.FirstName,
    ["lastName"]  = d => d.LastName,
    ["createdAt"] = d => d.CreatedAt,
};
```

### 1.13 🟡 Dashboard aggregations run in memory
`GetAppointmentReasonCounts` loads the whole `Appointments` table to run `GroupBy`. Push it to SQL:
`_context.Appointments.GroupBy(a => a.Reason).Select(g => new { g.Key, Count = g.Count() }).ToDictionaryAsync(...)`.
(Keep the sequential `await`s in `DashboardController`. `DbContext` isn't thread-safe, so don't switch to `Task.WhenAll`.)

### 1.14 🟡 Seeding fails on a second run
`SeedDoctorsAsync` and the other seeders always generate `doctor1@behealthy.com`, `doctor2@…` and so on. Seeding twice fails on duplicate email. Each entity is also committed separately, so a failure leaves the data half-seeded. Generate unique emails (Bogus is good for this) and wrap the batch in one transaction.

---

## 2. Error handling & result pattern

### 2.1 🟠 Remove the blanket `try { … } catch (Exception) { return Failed(...) }`
This appears in `AppointmentService`, `DepartmentService`, `PrescriptionService` and `SeedingService` (≈ 12 places). Problems:
- **Nothing is logged.** Bug 1.2 is invisible because of this.
- It catches `OperationCanceledException` and turns a client disconnect into a 400.
- It contradicts the rule in KNOWN_ISSUES #5 that exceptions go to the global handler.
- `SeedingService` returns `ex.Message` to the client, which leaks internals.

**Fix:** delete these blocks. Let `GlobalExceptionHandler` log the exception and map known types (§2.3). Only catch an exception you can actually handle.

### 2.2 🟠 Give `ServiceResponse` an error type and a payload (Result pattern)
Right now every failure is `400`, and `Create` returns `201` with no body and no `Location` header. Replace the current types with something like:
```csharp
public enum ErrorType { Validation, NotFound, Conflict, Forbidden }
public record Error(ErrorType Type, string Message, IDictionary<string,string[]>? Fields = null);
public record Result(bool Success, Error? Error) { … }
public record Result<T>(T? Value, …) : Result { … }
```
Then map it once in `ApiControllerBase`:
```csharp
protected IActionResult FromResult(Result r) => r.Error?.Type switch
{
    null                => NoContent(),
    ErrorType.NotFound  => NotFoundProblem(...),
    ErrorType.Conflict  => Problem(statusCode: 409, ...),
    ErrorType.Forbidden => Forbid(),
    _                   => ValidationProblem(...)
};
```
- Create methods return `Result<int>` (the new id), so controllers can return `CreatedAtAction(nameof(GetById), new { id }, null)`.
- This also removes `AuthResult`, which is a duplicate of `ServiceResponse<T>`.
- Make the write methods consistent. Today some return `Task` (Room, Specialty, MedicalRecord add and delete, Doctor/Nurse/Patient delete) and others return `ServiceResponse`.
- Libraries that do this: `ErrorOr`, `FluentResults`, `Ardalis.Result`. A ~40-line in-house version is fine too.

### 2.3 🟠 Make `GlobalExceptionHandler` map exception types
It currently returns 500 for everything. Add:
- `FluentValidation.ValidationException` → 400 `ValidationProblemDetails` with field errors
- `DbUpdateConcurrencyException` → 409
- `DbUpdateException` from an FK or unique-constraint violation → 409 with a generic message
- `KeyNotFoundException` or a custom `NotFoundException` → 404

### 2.4 🟡 Hard-coded English messages
`ServiceResponse.Failed(string errorMessage = "Something went wrong")`, all of `SeedingService`, `UserService` (`"User cannot be null"`, `"Invalid user role"`) and `AuthService` hard-code English. Everything else uses `Resource`. Use `Resource.SomethingWentWrong` and similar keys.

---

## 3. Validation — a concrete design

KNOWN_ISSUES #1 says to wire validation up. Some current problems stop that from working as-is.

### 3.1 🔴 Current validators can't be used on the server
- **Wrong target types.** `AppointmentDtoValidator`, `RoomDtoValidator`, `SpecialtyDtoValidator` and `ProfileDtoValidator` validate the *Response* DTOs (`AbstractValidator<AppointmentResponse>`), not `AppointmentCreateRequest`/`UpdateRequest`. Nothing validates appointment, room, specialty, department, prescription or medical-record requests.
- **Constructor flags.** `DoctorCreateDtoValidator(bool requiredSpecialty)`, `DoctorForUpdateDtoValidator(bool)` and `AppointmentDtoValidator(bool showNurses, bool showRooms)` take `bool` parameters, so DI can't build them. Those flags come from **AppSettings** (`DoNotAllowDoctorWithoutSpecialty`, `AppointmentRequiresRoom`). That means the business rules behind the admin settings are only enforced in the UI. A direct API call ignores them.

### 3.2 Recommended setup
1. **Move the validators into one shared place**, either a new `BeHealthy.Validation` project or `BeHealthy.Shared` with a FluentValidation dependency, so Front and API use the **same** classes. Today there are two copies (`Application/Validations/**` and `Front/Validations/**`) on **different FluentValidation versions** (12.1.1 vs 11.10.0).
2. **Split each validator into two layers.**
   - *Shape rules* (required, length, format, ranges) live in the shared validator and have no dependencies.
   - *Context rules* (AppSettings-driven, existence checks) live server-side, by injecting `IAppSettingsService` into a server validator and using `MustAsync`, or by keeping them in the service.
   ```csharp
   public sealed class DoctorCreateServerValidator : AbstractValidator<DoctorCreateRequest>
   {
       public DoctorCreateServerValidator(IAppSettingsService settings, ISpecialtyRepository specialties)
       {
           Include(new DoctorCreateShapeValidator());
           RuleFor(x => x.SpecialtyId)
               .NotNull()
               .WhenAsync(async (_, ct) => (await settings.GetSettingByKeyAsync(AppSettingKeys.DoNotAllowDoctorWithoutSpecialty, ct))?.GetBooleanValue() == true);
           RuleFor(x => x.SpecialtyId!.Value)
               .MustAsync(specialties.ExistsAsync).When(x => x.SpecialtyId.HasValue);
       }
   }
   ```
3. **Run validation automatically** with an MVC action filter, so no service can forget to call it:
   ```csharp
   public sealed class FluentValidationFilter(IServiceProvider sp) : IAsyncActionFilter
   {
       public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
       {
           foreach (var arg in ctx.ActionArguments.Values.Where(a => a is not null))
           {
               var validator = (IValidator?)sp.GetService(typeof(IValidator<>).MakeGenericType(arg!.GetType()));
               if (validator is null) continue;
               var result = await validator.ValidateAsync(new ValidationContext<object>(arg), ctx.HttpContext.RequestAborted);
               if (!result.IsValid) { ctx.Result = new BadRequestObjectResult(new ValidationProblemDetails(result.ToDictionary())); return; }
           }
           await next();
       }
   }
   // Program.cs
   builder.Services.AddValidatorsFromAssemblyContaining<DoctorCreateShapeValidator>();
   builder.Services.AddControllers(o => o.Filters.Add<FluentValidationFilter>());
   ```
   After that, `IValidatorService`/`ValidatorService` can be deleted.
4. **Remove `Blazored.FluentValidation` from `BeHealthy.Application`.** It is a Blazor UI package and doesn't belong in the application layer.

### 3.3 Specific rules to add or fix
- **SQLite does not enforce `HasMaxLength`.** None of the `.HasMaxLength(…)` calls in the EF configurations are enforced by the database. Validators are the **only** length guard, so every string with a max length in config needs a matching `MaximumLength`.
- `PATCH /api/medical-records/{id}/notes` takes `[FromBody] string notes` with no length check. Wrap it in a small request DTO such as `MedicalRecordNotesRequest(string Notes)` so it can be validated.
- Appointment: `EndTime > StartTime` (strict); date not in the past for new appointments; `NurseId` and `RoomId` exist; `Status` and `Reason` are defined enum values (`IsInEnum()`). `JsonStringEnumConverter` accepts integers outside the enum range.
- Prescription: `DatePrescribed` not in the future; `Medication` ≤ 100; `Dosage` ≤ 50.
- Visit, Treatment, Lab: `EndDate >= StartDate`; `ResultDate` not in the future.
- Password: Identity's `PasswordOptions` and the validator regexes define the policy separately. Configure `IdentityOptions.Password` once and treat Identity as the source of truth. Validators can stay for client-side UX, but keep the numbers in sync with a shared constant.
- Email: the regex `^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$` rejects valid addresses such as `a+b@x.com` and long TLDs. Use `.EmailAddress()`.
- AppSettings `PUT`: validate `Value` against `Type`. A `Checkbox` must parse as a bool and a `SingleSelect` must parse as an int. Bound `POST /settings/bulk` key lists, for example to 50 keys.
- `QueryParameters`: `PageNumber >= 1`, `1 <= PageSize <= 100`, `SearchTerm` ≤ 100.

### 3.4 🟠 Request DTOs carry fields the client shouldn't control (over-posting)
- `DoctorCreateRequest.UserId`, `NurseCreateRequest.UserId` and `PatientCreateRequest.UserId` are overwritten on the server, so they shouldn't be on the contract.
- `*UpdateRequest.UserId`: see 1.6.
- `DoctorUpdateRequest` has both `Specialty` (string) and `SpecialtyId`. The string is unused.
- `ConfirmPassword` is a UI concern. The API can ignore it or the Front can keep it in a view model.
- `MedicalRecord*.CreatedBy`: see 1.9.

---

## 4. Security

| | Issue | Fix |
|---|---|---|
| 🔴 | **JWT signing key is committed** in `BeHealthy.API/appsettings.json` | Move it to `dotnet user-secrets` or environment variables, **rotate it**, and fail startup if it's missing or shorter than 32 bytes. |
| 🔴 | List-all endpoints are open to `AllUsers` (patients): `GET /medical-records`, `GET /prescriptions`, `GET /visits`, `GET /patients`, `GET /appointments` | Restrict to `MedicalStaff`. Patients should only reach "my …" endpoints. |
| 🔴 | No ownership checks (KNOWN #2) | Use **resource-based authorization**: an `AuthorizationHandler<SameOrStaffRequirement, int /*patientId*/>` called through `IAuthorizationService.AuthorizeAsync(User, patientId, "PatientData")`, or a small `ICurrentUser` service so the application layer can scope queries itself. |
| 🟠 | A patient can `POST /appointments` for **any** `PatientId` | If the caller is a `Patient`, force `PatientId` to their own patient id. |
| 🟠 | No rate limiting on `/api/auth/login` | Use `builder.Services.AddRateLimiter(...)` with a fixed window per IP on the auth controller, together with lockout (KNOWN #3). |
| 🟠 | Seeding endpoints, hard-coded passwords (`Doctor123!`) and the seeded admin are available in **every** environment | Register `SeedingController` and the admin bootstrap only when `IsDevelopment()`, or behind a config flag. Read admin credentials from config. |
| 🟡 | CORS policy hard-codes `https://localhost:7130` | The Front is Blazor **Server** and calls the API server-to-server, so CORS is probably unnecessary. If you keep it, read origins from config. |
| 🟡 | 8-hour JWT with no refresh or revocation; role changes don't apply until expiry | Fine for a learning project. Shorter lifetime plus a refresh token if this ever goes further. |
| 🟡 | `DeleteDoctorAsync`, `DeleteNurseAsync` and `DeletePatientAsync` delete the user with `_context.Users.Remove` | This skips `UserManager`, so no Identity events or security-stamp updates. Use `UserManager.DeleteAsync` inside the transaction. |

---

## 5. Architecture & design smells

### 5.1 🟠 Layer dependencies point the wrong way
- **Domain → Shared.** `BeHealthy.Domain` references `BeHealthy.Shared` (the DTO/contract project) to get enums such as `AppointmentStatus` and `UserRole`. The core now depends on the API contract. Move the domain enums into Domain. If the frontend needs them, either keep Shared as a contracts project that *Domain doesn't reference*, or map them.
- **Domain → Identity EF Core.** `ApplicationUser : IdentityUser` puts `Microsoft.AspNetCore.Identity.EntityFrameworkCore` in Domain, along with an unused `Microsoft.Extensions.Caching.Memory`. That's a common pragmatic trade-off, but at least move `ApplicationUser` to Infrastructure and refer to users by `string UserId` in the domain.
- **UI enums in Domain.** `Domain/Enums.cs` has `PatientTabs`, `DepartmentTabs`, `MedicalRecordTabs` and `Severity`, which are UI tab enums. They belong in Front.
- **UI code in Application.** `EnumExtensions.GetBadgeClass`/`GetStatusColor` (CSS classes and hex colours), `DateTimeExtensions` formatting, `ValidationResultExtensions`, `ValueExtensions`, `AppSettingsConverterHelper` and the `Blazored.FluentValidation` package. Front has its own copies of most of these, so the Application versions are unused. Delete them.
- **Application depends on `UserManager<ApplicationUser>`** directly (`AuthService`, `UserService`). Optional improvement: put an `IIdentityService` in Application and implement it in Infrastructure.

### 5.2 🟠 Package & framework drift
- **Mixed target frameworks.** API, Infrastructure and Front target `net10.0`; Application, Domain, Shared and Tests target `net9.0`.
- **Mixed package versions.** `Identity.EntityFrameworkCore` is 9.0.8 in Domain and Application but 10.0.9 in Infrastructure. Tests use `EntityFrameworkCore.Sqlite` 9.0.8.
- **`Microsoft.AspNetCore.Identity` 2.3.1** is a **deprecated ASP.NET Core 2.x package**, referenced by Application, Infrastructure and Tests. Remove it and use `<FrameworkReference Include="Microsoft.AspNetCore.App" />` instead.
- Unneeded packages: `System.Text.Json`, `System.Text.Encodings.Web`, `System.Net.Http` 4.3.4, `System.Text.RegularExpressions` 4.3.1 (the old 4.3.x versions have known CVEs), and `QuickGrid` in **Infrastructure**.

**Fix:** add `Directory.Build.props` (shared `TargetFramework`, `Nullable`, `TreatWarningsAsErrors`, `AnalysisLevel=latest-recommended`) and `Directory.Packages.props` for **Central Package Management**.

### 5.3 🟠 The generic repository is leaky and partly wrong
- `QueryOptions<T>` (predicate, includes, order, paging, tracking) re-implements `IQueryable` badly. It can't project, can't do `ThenInclude`, and can't filter a count.
- `GetByUserIdAsync` uses `EF.Property<string>(e, "UserId")` for **any** `T`. On `Room` or `Specialty` it compiles and then throws at runtime.
- "Async" methods that aren't async: `UpdateAsync` and `DeleteEntityAsync` return `Task.CompletedTask`.
- Tracking is inconsistent. Some reads use `AsNoTracking`, many don't, so read-only queries track entities for no reason.
- Queries are duplicated. The `Include(Patient).Include(Doctor).Include(Room).Include(Nurse)` chain is copy-pasted 7 times across `AppointmentRepository`, `DoctorRepository.GetDoctorAppointmentsByUserIdAsync` and `PatientRepository.GetPatientAppointmentsByUserIdAsync`. The last two are appointment queries in the wrong repositories. `GetUserAppointmentsAsync` duplicates `GetAllAppointmentsByUserIdAsync`.

**Options, pick one:**
- **Specification pattern** (`Ardalis.Specification`). Keeps repositories but makes queries composable and reusable, for example `new AppointmentsForDoctorOnDateSpec(doctorId, date)`.
- **Drop the generic repository for reads.** Inject `ApplicationDbContext` into query services and **project straight to DTOs**:
  `_db.Doctors.AsNoTracking().Where(...).Select(d => new DoctorResponse { ... }).ToListAsync()`.
  This removes the include chains, the in-memory mapping and the over-fetching. The `*Simple` endpoints currently load full entities just to return id and name. Keep small repositories, or the DbContext, for writes. That is CQRS-lite and fits this codebase well.

### 5.4 🟠 Duplicated person/account logic (Doctor / Nurse / Patient)
- `AddDoctorAsync`, `AddNurseAsync` and `AddPatientAsync` are the same ~25 lines: build `ApplicationUser`, create it, add the role, map, save. The three `Update*Async` methods and three `Get*ProfileByUserIdAsync` methods are also copies.
- **Data is duplicated.** `FirstName` and `LastName` are stored on both `ApplicationUser` and `Doctor`/`Nurse`/`Patient`, and every update has to keep them in sync by hand. Choose one place for the names (the `User`), or use a shared `Person` base entity.
- **Fix:** extract an `IAccountProvisioningService.CreateAccountAsync(PersonCreateRequest, UserRole)` that returns the new user id, and use it from all three services. A generic `PersonService<TEntity>` is another option, but composition is simpler.

### 5.5 🟠 Some services return entities, others return DTOs
`VisitService` and `AppSettingsService` return **domain entities**, and the controllers map them using `BeHealthy.API/Mapping/*`. Every other service returns DTOs. `AppSettingsController.Update` also **changes the entity inside the controller** (`setting.Value = dto.Value`), which is business logic in the API layer. Make every service return DTOs and delete `API/Mapping`.

### 5.6 🟡 Mapper bloat
The Mappings classes contain many conversions that look unused: `Response → Domain`, `UpdateRequest → Response`, `Response → CreateRequest`, `MapToDomain(this DoctorUpdateRequest)`, and `MapToDomain(IEnumerable<…Response>)`. Most of them were probably needed by the old frontend, which now has its own `DtoMappers`. Remove what isn't referenced (check with *Find All References*). Keep mapping one-directional: `Request → Entity` for writes and `Entity → Response` for reads, or use projections (§5.3). If you want generated mappers, **Mapperly** gives compile-time mapping with no reflection.

### 5.7 🟡 Dead code (only referenced by interface + implementation)
`IUserService.RemoveUserFromRoleAsync`, `DeleteUserAsync` and `CreateAdminAsync` (the admin bootstrap uses `UserManager` directly instead); `IGenericRepository.DeleteEntityAsync`; `IAppointmentRepository.GetUserAppointmentsAsync`; `ISpecialtyRepository.GetAllSpecialtiesAsync`; `ILoggerService<T>`/`LoggerService<T>`, an unused wrapper around `ILogger<T>`, which is already an abstraction; `IValidatorService` (until §3 is done).

### 5.8 🟡 Cross-cutting concerns done by hand
- **Auditing.** `CreatedAt` is set in mappers, property initializers or not at all (1.9). Add an `IAuditable { CreatedAt, CreatedBy, ModifiedAt, ModifiedBy }` interface and a **`SaveChangesInterceptor`** that fills it from `TimeProvider` and the current user.
- **Time.** Inject **`TimeProvider`** instead of calling `DateTime.Now`/`UtcNow` directly. This makes "upcoming appointments" and the date rules testable.
- **Magic strings.** AppSetting keys (`"AppointmentRequiresRoom"`, and the typo `"DefaultDepartmentSupervison"`) and role names (`"Admin"` in `InitializeDatabaseAsync`). Add an `AppSettingKeys` constants class and use `nameof(UserRole.Admin)`.

### 5.9 🟡 Healthcare-specific suggestions
- **Soft delete** for clinical entities (Patient, Visit, MedicalRecord, Prescription, Allergy, Appointment): an `IsDeleted` flag plus an EF **global query filter**. Hard deletes and cascades lose history.
- **Optimistic concurrency.** Two staff editing the same record cause a silent last-write-wins. Add a concurrency token (`Guid Version` with `.IsConcurrencyToken()`, since SQLite has no `rowversion`) and return 409 on conflict.
- **Audit log** of who read or changed a patient's data. This is a real requirement in medical software and easy to add with the interceptor above.

### 5.10 🟡 Smaller consistency items
- Services use explicit constructor and field boilerplate while controllers use primary constructors. Pick one style.
- `DoctorService.UpdateDoctorAsync` returns `Resource.NotFound` when the *specialty* is missing, so the message is ambiguous. `DepartmentId` is never checked for existence, so a bad id becomes an FK exception and a 500.
- `GET /api/appointments/upcoming` is missing `///` docs and `[ProducesResponseType]`, unlike every other action.
- `AppSettingsController.Update` duplicates the id-mismatch logic for string keys. Add an `EnsureMatching<T>` overload.
- Two solution files (`BeHealthy.sln`, `Front.sln`). Consider a single `.slnx`.

---

## 6. Tests & CI

- 🔴 **The CI workflow can't work.** `.github/workflows/docker-image.yml` triggers on `master` (the branch is `main`) and builds `BeHealthy/BeHealthy/Dockerfile`, which isn't in the repo. It also never runs `dotnet build` or `dotnet test`. Replace it with a `dotnet restore / build / test` job, and add the Docker push afterwards if you still want it.
- 🟠 **Coverage is thin.** Unit tests exist for four services (Appointment, Department, Doctor, Patient: ~73 tests), mocking repositories. There are no tests for validators, authorization rules or EF mappings. These would have caught 1.1, 1.2, 1.3 and 1.5:
  - **Integration tests** with `WebApplicationFactory<Program>` and SQLite in-memory (`DataSource=:memory:`, keep the connection open). Real FK and cascade behaviour, real JSON, real auth.
  - An **authorization matrix test**: for each endpoint × role, assert the expected 200/403. This protects the `RoleGroups` work from regressions.
  - **Validator tests** with `FluentValidation.TestHelper` (`TestValidate(...).ShouldHaveValidationErrorFor(...)`).
- 🟡 The Tests project targets `net9.0` while the API targets `net10.0`, so you can't reference the API for `WebApplicationFactory` until they're aligned (§5.2).

---

## F. Frontend (secondary)

- 🟠 KNOWN #7 is still open: `PostAsync`, `PutAsync` and `DeleteAsync` ignore the response. `GetAsync` also turns any `HttpRequestException` into `default`, so a network failure looks the same as "not found" or "empty list".
- 🟠 **Duplicated code** between Front and Application: validators (different FluentValidation versions), `AppSettingsConverterHelper`, `EnumExtensions`, `DateTimeExtensions`, `ValueExtensions` and `ValidationResultExtensions`. Shared, UI-agnostic code goes in one project. UI-only code stays in Front only.
- 🟡 **Two toast systems** (`ToastService` and `ToastrStateService`), both registered. Finish the migration and delete one.
- 🟡 KNOWN #10: `Settings.razor` still has no `@attribute [Authorize(Roles = "Admin")]`. Consider `AuthorizeRouteView` in `Routes.razor` instead of the layout redirect.
- 🟡 `Doctors/Create.razor` (227 lines), `Nurses/Create.razor`, `Patients/Create.razor` and the matching `Edit.razor` pages are near-copies. Extract a shared `PersonForm` component with role-specific slots, for example the specialty picker for doctors.

---

## Suggested order of attack

1. Fix the cheap, high-impact bugs: **1.1** (counts), **1.2** (prescriptions), **1.3** (EF configs + migration), **1.5** (overlap operator + cancelled), **1.6** (UserId trust), **1.7** (`FirstAsync`).
2. Remove the blanket `catch (Exception)` (**2.1**) and extend `GlobalExceptionHandler` (**2.3**), so bugs show up in the logs.
3. Introduce the `Result`/`ErrorType` type (**2.2**). Every later fix becomes a one-line return.
4. Set up the validation pipeline (**§3**), including AppSettings-driven rules on the server.
5. Security: move the JWT key out of source, lock down the list endpoints, add the ownership handler, lockout and rate limiting (**§4**).
6. Housekeeping: `Directory.Build.props` and Central Package Management, delete dead code, fix CI (**§5.2, 5.7, §6**).
7. Bigger refactors as time allows: projections or specifications (**5.3**), account provisioning (**5.4**), auditing interceptor and soft delete (**5.8–5.9**).
