# Code Review — Smells, Bad Practices & Improvements

> Written 2026-10-02 against `main` @ `637fdc7`. **Status re-checked 2026-10-06 against `main` @ `6457947`; items 1.1–1.8 fixed afterwards in the working tree.** Paths below are relative to `src/` unless noted.
> This adds to [`KNOWN_ISSUES.md`](KNOWN_ISSUES.md) rather than repeating it. Where an item overlaps, it says so.
> Ordered by priority inside each section. **Backend first**, frontend notes at the end.
> Legend: 🔴 bug / data risk · 🟠 design smell with real consequences · 🟡 cleanup / nice to have
> Status: ✅ fixed · ◐ partly fixed · ⬜ still open · ➖ no longer applicable

---

## 0. Status of `KNOWN_ISSUES.md` items

| # | Item | Status now |
|---|------|-----------|
| 1 | No server-side validation | ✅ **Fixed.** §3 was implemented; see [`VALIDATION.md`](VALIDATION.md). `ValidationFilter` is registered globally (`BeHealthy.API/Program.cs:15`). |
| 2 | IDOR / missing authorization | ◐ **Partly fixed.** Role attributes (`RoleGroups`) exist, and the new `MeController` (`api/me/...`) scopes by the token's user id. The broad endpoints still have no ownership checks. A `Patient` can still `GET /api/medical-records`, `GET /api/prescriptions` and `GET /api/patients/{id}/...` for **any** patient. See §4. |
| 3 | Login bypasses lockout | ⬜ Still open (`BeHealthy.Application/Services/AuthService.cs:24`, `CheckPasswordAsync`). `SignInManager` is already registered (`BeHealthy.Infrastructure/DependencyInjection.cs:58`), so switching to it is a one-liner. |
| 4 | DB wiped on startup | ◐ **Fixed in `6457947`, but the fix creates a new bug.** Both `EnsureDeleted()` **and** `Migrate()` were removed from `InitializeDatabaseAsync`. A fresh checkout now has no schema, and the admin seed fails until you run `dotnet ef database update` by hand. Put `context.Database.MigrateAsync()` back (keep `EnsureDeleted` out). `context` and `roleManager` are now unused there. `README.md` still says the DB is reset on every start. |
| 6 | Singleton `ToastService` | ✅ **Fixed.** It is now `Scoped`. Both toast services still exist (see §F). |
| 7 | Fire-and-forget API calls | ◐ **Partly fixed.** Every create and update now uses `*ForResponseAsync`. Seven deletes (Appointment, Doctor, MedicalRecord, Nurse, Patient, Room, Specialty) still use the fire-and-forget `ApiClientBase.DeleteAsync`. Since 1.3 and 1.7 the API can refuse these deletes (clinical history, not found), but the UI just reloads the list and shows no message. |
| 8 | Missing FK checks | ✅ **Fixed** by the server validators (§3). `Prescription.TreatmentId` is now optional (1.2). |
| 9 | Double-booking race | ⬜ Still open. §1.5 covers more conflict-check bugs. |
| 10 | Settings page not gated | ⬜ Still open. `Settings.razor` has no `[Authorize]`. The API side is protected (`AppSettingsController` Update is Admin-only). |

---

## 1. Backend correctness bugs

### 1.1 🔴 Pagination `TotalCount` ignores the filter — ✅ fixed
`AppointmentService.GetAllAppointmentsAsync`, `NurseService.GetAllNursesAsync` and `DoctorService.GetAllDoctorsAsync` counted the **whole table**, so the paginator showed pages for every row while searching or filtering. `DoctorService` also loaded every matching doctor into an unused `allDoctors` list.

**Done:** `IGenericRepository` has a `GetCountAsync(Expression<Func<T,bool>> predicate)` overload, and all three services pass it the same predicate as the page query. The unused `allDoctors` query is gone. Unit tests check that the count predicate matches the filter.

### 1.2 🔴 Creating a prescription can never succeed — ✅ fixed
`Prescription.TreatmentId` was a **required** FK, but `PrescriptionCreateRequest` has no `TreatmentId`, so every insert wrote `TreatmentId = 0` and failed the FK check. The blanket `catch (Exception)` hid the cause.

**Done:** `TreatmentId` is now optional (`int?`, `SetNull`), because the API has no way to create treatments (migration `MakePrescriptionTreatmentOptional`). Still open: the `catch (Exception)` in `PrescriptionService` (§2.1).

### 1.3 🔴 Conflicting EF relationship configurations — ✅ fixed
The same relationship was configured in two places with different delete behaviour (Visit → Patient, Room → Department, Nurse → Department twice, Doctor → Appointments), so the result depended on configuration order. A patient with visits gave a 500 on delete, and deleting a doctor cascade-deleted all their appointments.

**Done:**
- Each relationship is configured once, on the dependent side. The principal-side `HasMany` duplicates in Doctor, Nurse, Patient, Department, Room, Visit and Diagnosis configurations are gone.
- Clinical history is `Restrict` (migration `RestrictDeletesOfClinicalHistory`): Appointment → Patient/Doctor, Allergy → Patient, MedicalRecord → Patient, Prescription → Patient/Doctor, and Visit → MedicalRecord. Visit → Patient/Doctor were already Restrict.
- The delete services check first and refuse with a clear message instead of a 500:
  - Patient: any appointments, visits, medical records, prescriptions or allergies (`IPatientRepository.HasClinicalHistoryAsync`).
  - Doctor: any appointments, visits or prescriptions (`IDoctorRepository.HasClinicalHistoryAsync`).
  - Medical record: any visits.
- Unchanged on purpose: Nurse and Room on an appointment are `SetNull`, and a visit's diagnoses, lab results and treatments cascade with the visit.
- Soft delete (§5.9) is still the long-term answer for removing people who have history.

### 1.4 🔴 `DateTime` kind conversion relabels instead of converting — ✅ fixed
`ApplicationUser.DateOfBirth` and `Prescription.DatePrescribed` were written labelled `Utc` and read back labelled `Local`. SQLite stores the text unchanged, so the read value was then serialized with the server's offset.

**Done:** both converters are removed. These are calendar dates picked in the UI, so they are stored and returned exactly as entered (`Kind = Unspecified`, no offset in the JSON). No migration was needed. Converting `DateOfBirth` to `DateOnly` is still a nice-to-have.

### 1.5 🔴 Appointment conflict check: several bugs — ✅ fixed
- ✅ **Back-to-back slots** no longer conflict (`<` and `>`).
- ✅ **Cancelled appointments** no longer block a slot. Saving an appointment whose own status is `Cancelled` skips the check.
- ✅ **Runs in the database.** `OverlappingAppointments` builds one predicate (same day, not cancelled, overlapping, not itself), and `FindConflictAsync` runs it per doctor, patient, nurse and room, fetching at most one row. The unused `GetAllAppointmentsByNurseIdAsync` and `RoomRepository.GetRoomAppointmentsAsync` are deleted.
- ✅ **End > start** is enforced by the shared validators. The service itself still doesn't check, so internal callers such as `SeedingService` skip the rule.
- ✅ **`NurseId` existence** is checked by `AppointmentCreate/UpdateRequestServerValidator`.
- Tests now run the real predicate against an in-memory store. They cover doctor, patient, nurse and room conflicts, back-to-back slots, cancelled appointments, another day, and an update that only overlaps itself. The old update-conflict tests passed only because the appointment lookup was never set up.
- The race condition is still open (KNOWN_ISSUES #9).

### 1.6 🔴 Updating a doctor, nurse or patient trusts two independent ids — ✅ fixed
The update loaded the user by `dto.UserId` and the entity by `dto.Id` without checking they belonged together, so one request could rename someone else's account.

**Done:** `UserId` is removed from `DoctorUpdateRequest`, `NurseUpdateRequest` and `PatientUpdateRequest`. The services load the entity with `GetByIdWithIncludes(id, x => x.User)` and update that user. The ambiguous `Resource.NotFound` messages now name the missing entity (doctor, nurse, patient or specialty).

### 1.7 🔴 Not-found handling is inconsistent, and some of it silently succeeds — ◐ mostly fixed
- ✅ `DepartmentRepository.GetDepartmentByIdAsync` uses `FirstOrDefaultAsync` and returns `Department?`, so the controller's 404 branch works.
- ✅ Deleting an appointment, room, specialty, medical record, doctor, nurse or patient now returns a `ServiceResponse`. A missing id reports "… was not found" instead of 204. The repository `Delete{Doctor,Nurse,Patient}Async` methods return `bool`.
- ✅ `UpdateMedicalRecordNotesAsync` returns a `ServiceResponse` and reports a missing record.
- ⬜ Every failure is still **400** from `ProblemFromServiceResponse`, including not-found. That waits for the error type in §2.2.

### 1.8 🟠 Upcoming appointments have no doctor or patient — ✅ fixed
**Done:** `GetUpcomingAppointmentsAsync` includes `Doctor` and `Patient`, and sets `PageNumber = 1`. Before, `PageSize = 5` was ignored without a page number, so the widget got every appointment in the 3-day window.

`DateTime.Now` is kept on purpose. Appointment dates and times are clinic wall-clock values, so "today" is the local date, and `UtcNow` would be wrong near midnight.

### 1.9 🟠 Audit fields are set by the client — ⬜ open
- `MedicalRecordCreateRequest.CreatedBy` and `MedicalRecordUpdateRequest.CreatedBy` come from the request body (`MedicalRecordService.cs:46`, `MedicalRecordMapper.cs:39`), so anyone can claim anyone wrote a record. `CreatedUserId` is never set. Both should come from `User.FindFirstValue(ClaimTypes.NameIdentifier)` on the server.
- `AllergyUpdate` (`AllergyService.cs:43`), `MedicalRecordUpdate` (`MedicalRecordService.cs:43`) and `VisitUpdate` (`VisitMapper.cs:26`) let the client **move the record to another patient** (`PatientId = dto.PatientId`). An update should not be able to re-parent a clinical record.
- `Room.CreatedAt` and `Specialty.CreatedAt` are **never set**, so they hold `0001-01-01`. Doctors, nurses and patients set it in the mapper, and `Department` uses a property initializer. Three different conventions for one concern; see §5.8.

### 1.10 🟠 Patients endpoint is unbounded and ignores its own parameters — ⬜ open
`GET /api/patients` returns `IEnumerable` with no paging, unlike doctors and nurses. `PatientService.GetAllPatientsAsync` only uses `SearchTerm`. `PatientQueryParameters.FirstName`, `LastName`, `OrderBy`, `PageNumber` and `PageSize` are accepted and sent by the frontend, but **ignored**. The new `GET /api/me/patients` is also unpaged.

### 1.11 🟠 Paging input isn't clamped — ✅ fixed
`QueryParametersValidator` now enforces `PageNumber >= 1` and `1 <= PageSize <= 100`, and `ValidationFilter` validates `[FromQuery]` objects. `PaginatedResult.TotalPages` would still divide by zero if code inside the app passed `PageSize = 0`, but API input is guarded.

### 1.12 🟠 `OrderBy` accepts any property path from the client — ⬜ open
`OrderByHelper` builds `x => x.<anything>` from a query-string value. Validation only limits its length to 50. `?OrderBy=User.PasswordHash` or `User.SecurityStamp` gives an attacker a sorting oracle over sensitive columns. **Allow-list** the sortable columns per entity:
```csharp
private static readonly Dictionary<string, Expression<Func<Doctor, object>>> Sorts = new(StringComparer.OrdinalIgnoreCase)
{
    ["firstName"] = d => d.FirstName,
    ["lastName"]  = d => d.LastName,
    ["createdAt"] = d => d.CreatedAt,
};
```

### 1.13 🟡 Aggregations run in memory — ⬜ open
`GetAppointmentReasonCounts` loads the whole `Appointments` table to run `GroupBy`. Push it to SQL:
`_context.Appointments.GroupBy(a => a.Reason).Select(g => new { g.Key, Count = g.Count() }).ToDictionaryAsync(...)`.
(Keep the sequential `await`s in `DashboardController`. `DbContext` isn't thread-safe, so don't switch to `Task.WhenAll`.)

The new `DoctorService.GetMyPatientsAsync` and `PatientService.GetMyDoctorsAsync` follow the same pattern. They load every appointment for the user and then query again. Use one query with `Select(...).Distinct()`.

### 1.14 🟡 Seeding fails on a second run — ◐ partly fixed
- ✅ **Unique emails.** `Seeding/SeedDataGenerator.cs` now uses Bogus with a random suffix. A rare collision just fails that one item.
- ⬜ **No transaction.** Each entity is still committed separately, and `SeedAllAsync` runs the four seeders independently, so a failure leaves the data half-seeded.
- ⬜ Seeding calls the services directly, so the FluentValidation rules don't run on seeded data.

---

## 2. Error handling & result pattern

### 2.1 🟠 Remove the blanket `try { … } catch (Exception) { return Failed(...) }` — ⬜ open
There are still 11 of these: `AppointmentService` (`:143`, `:209`), `DepartmentService` (`:49`, `:73`, `:123`), `PrescriptionService` (`:41`, `:68`) and `SeedingService` (`:99`, `:129`, `:159`, `:208`). Problems:
- **Nothing is logged.** Bug 1.2 is invisible because of this.
- It catches `OperationCanceledException` and turns a client disconnect into a 400, so the new 499 mapping in `GlobalExceptionHandler` never sees those cases.
- It contradicts the rule in KNOWN_ISSUES #5 that exceptions go to the global handler.
- `SeedingService` returns `ex.Message` to the client, which leaks internals.

**Fix:** delete these blocks. Let `GlobalExceptionHandler` log the exception and map known types (§2.3). Only catch an exception you can actually handle.

### 2.2 🟠 Give `ServiceResponse` an error type and a payload (Result pattern) — ⬜ open
Right now every failure is `400`, and `Create` returns `201` with no body and no `Location` header. `ServiceResponse` gained `ValidationErrors` for the Front, but still has no error type or value. Replace the current types with something like:
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
- Make the write methods consistent. Every delete and the medical-record notes update now return `ServiceResponse` (1.7). Still returning a plain `Task`: MedicalRecord, Room and Specialty add, and `AppSettingsService.UpdateSettingAsync`.
- Libraries that do this: `ErrorOr`, `FluentResults`, `Ardalis.Result`. A ~40-line in-house version is fine too.

### 2.3 🟠 Make `GlobalExceptionHandler` map exception types — ◐ partly fixed
It already maps:
- ✅ `FluentValidation.ValidationException` → 400 `ValidationProblemDetails` with field errors
- ✅ `OperationCanceledException` on an aborted request → 499

Still missing:
- ⬜ `DbUpdateConcurrencyException` → 409
- ⬜ `DbUpdateException` from an FK or unique-constraint violation → 409 with a generic message
- ⬜ `KeyNotFoundException` or a custom `NotFoundException` → 404

### 2.4 🟡 Hard-coded English messages — ⬜ open
These still hard-code English:
- `ServiceResponse.Failed(string errorMessage = "Something went wrong")`
- `UserService` (`"User cannot be null"`, `"Invalid user role"`, `"User not found"`)
- `AuthService` (`"Invalid username or password."`)
- every message in `SeedingService`

Everything else, including the new validators, uses `Resource`. Use `Resource.SomethingWentWrong` and similar keys.

---

## 3. Validation — a concrete design

> ✅ **Implemented** (commit `70a1f3a`). The design below is kept for reference; [`VALIDATION.md`](VALIDATION.md) describes what was built. Differences from this sketch: the shared validators live in a new `BeHealthy.Validation` project; setting rules go through an `IValidationSettingsProvider` abstraction, so they run on the Front too; and server validators are separate `*ServerValidator` classes (the filter runs every registered validator) rather than `Include(...)` wrappers. §3.4 (over-posting) and the open rules in §3.3 are still open.

### 3.1 🔴 Current validators can't be used on the server — ✅ fixed
- **Wrong target types.** `AppointmentDtoValidator`, `RoomDtoValidator`, `SpecialtyDtoValidator` and `ProfileDtoValidator` validated the *Response* DTOs. Now there is one validator per request DTO, and `ValidatorRegistrationTests` fails when a request type has none.
- **Constructor flags.** The `bool` constructor flags are gone. AppSettings-driven rules now read the setting through `IValidationSettingsProvider`, so a direct API call is checked too.

### 3.2 Recommended setup — ✅ done (see `VALIDATION.md`)
1. Validators are in one shared project (`BeHealthy.Validation`, FluentValidation 12.1.1), used by Front and API.
2. Shape rules are shared, and context rules live in `*ServerValidator` classes in Application.
3. `ValidationFilter` runs automatically for every action argument. `IValidatorService` is deleted.
4. `Blazored.FluentValidation` was removed from `BeHealthy.Application`.

### 3.3 Specific rules to add or fix
- ✅ **SQLite does not enforce `HasMaxLength`.** Every request DTO field now has a `MaximumLength` from `FieldLengths`, and the EF configurations use the same constants. Literal lengths remain only on entities with no request DTO (Diagnosis, LabResult, Treatment, AppSetting.Caption).
- ✅ `PATCH /api/medical-records/{id}/notes` now takes `MedicalRecordNotesUpdateRequest`, which is validated.
- ◐ Appointment: ✅ `EndTime > StartTime` (strict); ✅ `NurseId` and `RoomId` exist; ✅ `Status` and `Reason` are defined enum values. ⬜ **Date not in the past for new appointments is still missing.**
- ✅ Prescription: `DatePrescribed` not in the future; `Medication` ≤ 100; `Dosage` ≤ 50.
- ➖ Visit, Treatment, Lab `EndDate >= StartDate` / `ResultDate`: no request DTO carries these fields, so the API can't write them. ⬜ `VisitDate` has no not-in-the-future rule.
- ✅ Password: `PasswordPolicy` constants configure Identity and the validator rules.
- ✅ Email: uses `.EmailAddress()` plus a max length.
- ◐ AppSettings: ✅ `PUT` validates `Value` against `Type`. ⬜ `POST /settings/bulk` (`List<string> keys`) is still unbounded. There is no validator for a bare list, and the endpoint is open to `AllUsers`. Wrap it in a request DTO.
- ✅ `QueryParameters`: `PageNumber >= 1`, `1 <= PageSize <= 100`, `SearchTerm` ≤ 100.

### 3.4 🟠 Request DTOs carry fields the client shouldn't control (over-posting) — ⬜ open
- `DoctorCreateRequest.UserId`, `NurseCreateRequest.UserId` and `PatientCreateRequest.UserId` are overwritten on the server, so they shouldn't be on the contract.
- ✅ `*UpdateRequest.UserId` is removed (1.6).
- `DoctorUpdateRequest` has both `Specialty` (string) and `SpecialtyId`. The string is unused.
- `ConfirmPassword` is a UI concern. It is now also checked on the server (`.Equal(x => x.Password)`), which is harmless, but the Front could keep it in a view model instead.
- `MedicalRecord*.CreatedBy` and `*UpdateRequest.PatientId`: see 1.9.

---

## 4. Security

| | Issue | Status | Fix |
|---|---|---|---|
| 🔴 | **JWT signing key is committed** in `BeHealthy.API/appsettings.json` | ⬜ | Move it to `dotnet user-secrets` or environment variables, **rotate it**, and fail startup if it's missing or shorter than 32 bytes (`Program.cs:38` uses `jwtSection["Key"]!` with no check). |
| 🔴 | Broad endpoints are open to `AllUsers` (patients): `GET /medical-records`, `GET /prescriptions`, `GET /visits`, `GET /patients`, `GET /appointments`, `/patients/{id}/appointments\|allergies\|prescriptions\|medical-records\|visits`, `/visits/{id}/...`, `GetById` on records, prescriptions and visits | ⬜ | Restrict to `MedicalStaff`. Patients now have `api/me/...` endpoints, so the broad ones can be closed to them. |
| 🔴 | No ownership checks (KNOWN #2) | ◐ | `MeController` covers the `/me` routes only. For the rest, use **resource-based authorization**: an `AuthorizationHandler<SameOrStaffRequirement, int /*patientId*/>` called through `IAuthorizationService.AuthorizeAsync(User, patientId, "PatientData")`, or a small `ICurrentUser` service so the application layer can scope queries itself. |
| 🟠 | A patient can `POST /appointments` for **any** `PatientId` | ⬜ | If the caller is a `Patient`, force `PatientId` to their own patient id. |
| 🟠 | No rate limiting on `/api/auth/login` | ⬜ | Use `builder.Services.AddRateLimiter(...)` with a fixed window per IP on the auth controller, together with lockout (KNOWN #3). |
| 🟠 | Seeding endpoints, hard-coded passwords (`Doctor123!`) and the seeded admin are available in **every** environment | ◐ | ✅ The admin bootstrap now runs only in Development. ⬜ `SeedingController` is still registered everywhere (Admin-only). ⬜ Admin credentials (`Infrastructure/DependencyInjection.cs:76`, `:87`) and seed passwords (`SeedDataGenerator.cs:12-14`) are hard-coded; read them from config. |
| 🟡 | CORS policy hard-codes `https://localhost:7130` | ⬜ | The Front is Blazor **Server** and calls the API server-to-server, so CORS is probably unnecessary. If you keep it, read origins from config. |
| 🟡 | 8-hour JWT with no refresh or revocation; role changes don't apply until expiry | ⬜ | Fine for a learning project. Shorter lifetime plus a refresh token if this ever goes further. |
| 🟡 | Doctor, Nurse and Patient delete remove the user with `_context.Users.Remove` (`DoctorRepository.cs:37`, `NurseRepository.cs:31`, `PatientRepository.cs:35`) | ⬜ | This skips `UserManager`, so no Identity events or security-stamp updates. Use `UserManager.DeleteAsync` inside the transaction. |

---

## 5. Architecture & design smells

### 5.1 🟠 Layer dependencies point the wrong way — ◐ partly fixed
- ⬜ **Domain → Shared.** `BeHealthy.Domain` references `BeHealthy.Shared` (the DTO/contract project) to get enums such as `AppointmentStatus` and `AllergySeverity`. The core now depends on the API contract. Move the domain enums into Domain. If the frontend needs them, either keep Shared as a contracts project that *Domain doesn't reference*, or map them.
- ⬜ **Domain → Identity EF Core.** `ApplicationUser : IdentityUser` puts `Microsoft.AspNetCore.Identity.EntityFrameworkCore` in Domain, along with an unused `Microsoft.Extensions.Caching.Memory`. That's a common pragmatic trade-off, but at least move `ApplicationUser` to Infrastructure and refer to users by `string UserId` in the domain.
- ◐ **UI enums in Domain.** Front now has its own `Models/UiEnums.cs` and `Models/Severity.cs`. The old copies in `Domain/Enums.cs` (`Severity`, `MedicalRecordTabs`, `PatientTabs`, `DepartmentTabs`) are unreferenced and can be deleted.
- ◐ **UI code in Application.** `ValidationResultExtensions` and the `Blazored.FluentValidation` package are gone. Still there and unused: `Extensions/EnumExtensions.cs` (`GetBadgeClass`/`GetStatusColor`), `Extensions/DateTimeExtensions.cs` and `Extensions/ValueExtensions.cs`. `Helpers/AppSettingsConverterHelper.cs` is used only for `GetBooleanValue`. Front has its own copies of all of these.
- ⬜ **Application depends on `UserManager<ApplicationUser>`** directly (`AuthService`, `UserService`). Optional improvement: put an `IIdentityService` in Application and implement it in Infrastructure.
- New: Application references `Bogus` for seed data. Fine for now, but a candidate to move into a dev-only project or `Infrastructure`.

### 5.2 🟠 Package & framework drift — ⬜ open
- **Mixed target frameworks.** API, Infrastructure and Front target `net10.0`. Application, Domain, Shared, the new Validation project and Tests target `net9.0`.
- **Mixed package versions.** Version pairs that differ between projects:
  - `Identity.EntityFrameworkCore`: 9.0.8 in Domain and Application, 10.0.9 in Infrastructure.
  - `Caching.Memory`: 9.0.8 / 10.0.9.
  - `EntityFrameworkCore.Sqlite`: 9.0.8 in Tests, 10.0.9 in Infrastructure.
  - `System.Text.Json`: 9.0.8 / 10.0.9.
  - `System.Text.Encodings.Web`: 8.0.0 / 10.0.9.
  - `QuickGrid`: 9.0.8 / 10.0.8 / 10.0.9.
- **`Microsoft.AspNetCore.Identity` 2.3.1** is a **deprecated ASP.NET Core 2.x package**, still referenced by Application, Infrastructure and Tests. Remove it and use `<FrameworkReference Include="Microsoft.AspNetCore.App" />` instead.
- Unneeded packages:
  - `System.Text.Json`, `System.Text.Encodings.Web` and `System.Security.Cryptography.Xml` in Application and Infrastructure
  - `QuickGrid` in **Infrastructure** and Tests
  - `System.Net.Http` 4.3.4 and `System.Text.RegularExpressions` 4.3.1 in Tests (the old 4.3.x versions have known CVEs)
  - `Newtonsoft.Json` in Tests

**Fix:** `Directory.Build.props` now exists but only sets `EnforceCodeStyleInBuild`. Add the shared `TargetFramework`, `Nullable`, `TreatWarningsAsErrors` and `AnalysisLevel=latest-recommended`, and add `Directory.Packages.props` for **Central Package Management**.

### 5.3 🟠 The generic repository is leaky and partly wrong — ◐ partly fixed
- ⬜ `QueryOptions<T>` (predicate, includes, order, paging, tracking) re-implements `IQueryable` badly. It can't project, can't do `ThenInclude`, and can't filter a count.
- ⬜ `GetByUserIdAsync` uses `EF.Property<string>(e, "UserId")` for **any** `T` (`GenericRepository.cs:82`). On `Room` or `Specialty` it compiles and then throws at runtime.
- ⬜ "Async" methods that aren't async: `UpdateAsync` and `DeleteEntityAsync` return `Task.CompletedTask`.
- ⬜ Tracking is inconsistent. Some reads use `AsNoTracking`, many don't, so read-only queries track entities for no reason.
- ◐ Duplicated queries. The appointment queries in `DoctorRepository` and `PatientRepository` are gone. The `Include(Patient).Include(Doctor).Include(Room).Include(Nurse)` chain is still copy-pasted 5 times in `AppointmentRepository`, and `GetUserAppointmentsAsync` still duplicates `GetAllAppointmentsByUserIdAsync`.

**Options, pick one:**
- **Specification pattern** (`Ardalis.Specification`). Keeps repositories but makes queries composable and reusable, for example `new AppointmentsForDoctorOnDateSpec(doctorId, date)`.
- **Drop the generic repository for reads.** Inject `ApplicationDbContext` into query services and **project straight to DTOs**:
  `_db.Doctors.AsNoTracking().Where(...).Select(d => new DoctorResponse { ... }).ToListAsync()`.
  This removes the include chains, the in-memory mapping and the over-fetching. The `*Simple` endpoints currently load full entities just to return id and name. Keep small repositories, or the DbContext, for writes. That is CQRS-lite and fits this codebase well.

### 5.4 🟠 Duplicated person/account logic (Doctor / Nurse / Patient) — ⬜ open
- `AddDoctorAsync`, `AddNurseAsync` and `AddPatientAsync` are the same ~25 lines: build `ApplicationUser`, create it, add the role, map, save. The three `Update*Async` methods and three `Get*ProfileByUserIdAsync` methods are also copies.
- **Data is duplicated.** `FirstName` and `LastName` are stored on both `ApplicationUser` and `Doctor`/`Nurse`/`Patient`, and every update has to keep them in sync by hand. Choose one place for the names (the `User`), or use a shared `Person` base entity.
- **Fix:** extract an `IAccountProvisioningService.CreateAccountAsync(PersonCreateRequest, UserRole)` that returns the new user id, and use it from all three services. A generic `PersonService<TEntity>` is another option, but composition is simpler.

### 5.5 🟠 Some services return entities, others return DTOs — ⬜ open
`VisitService` and `AppSettingsService` return **domain entities**, and the controllers map them using `BeHealthy.API/Mapping/*`. Every other service returns DTOs. `AppSettingsController.Update` also **changes the entity inside the controller** (`setting.Value = dto.Value`, `:56`), which is business logic in the API layer. Make every service return DTOs and delete `API/Mapping`.

### 5.6 🟡 Mapper bloat — ⬜ open
The Mappings classes contain many conversions that look unused: `Response → Domain`, `UpdateRequest → Response`, `Response → CreateRequest`, `MapToSelf(RoomResponse)`, and `MapToDomain(IEnumerable<…Response>)`. Most of them were probably needed by the old frontend, which now has its own `DtoMappers`. Remove what isn't referenced (check with *Find All References*). Keep mapping one-directional: `Request → Entity` for writes and `Entity → Response` for reads, or use projections (§5.3). If you want generated mappers, **Mapperly** gives compile-time mapping with no reflection.

### 5.7 🟡 Dead code (only referenced by interface + implementation) — ◐ partly fixed
- ⬜ `IUserService.RemoveUserFromRoleAsync`, `DeleteUserAsync` and `CreateAdminAsync` (the admin bootstrap uses `UserManager` directly instead)
- ⬜ `IGenericRepository.DeleteEntityAsync`
- ⬜ `IAppointmentRepository.GetUserAppointmentsAsync`
- ⬜ `ILoggerService<T>`/`LoggerService<T>`: an unused wrapper around `ILogger<T>`, which is already an abstraction. It is only registered in DI.
- ⬜ Front: `States/ToastrStateService` (see §F) and the Domain UI enums (see §5.1)
- ➖ `ISpecialtyRepository.GetAllSpecialtiesAsync` is now used by `SeedingService`.
- ✅ `IValidatorService` was deleted.

### 5.8 🟡 Cross-cutting concerns done by hand — ◐ partly fixed
- ⬜ **Auditing.** `CreatedAt` is set in mappers, property initializers or not at all (1.9). Add an `IAuditable { CreatedAt, CreatedBy, ModifiedAt, ModifiedBy }` interface and a **`SaveChangesInterceptor`** that fills it from `TimeProvider` and the current user.
- ⬜ **Time.** Inject **`TimeProvider`** instead of calling `DateTime.Now`/`UtcNow` directly. This makes "upcoming appointments" and the date rules testable.
- ◐ **Magic strings.**
  - ✅ `Shared/Common/AppSettingKeys.cs` now holds the setting keys.
  - ⬜ The typo `DefaultDepartmentSupervison` is still in both the constant name and its value (renaming the value needs a data migration).
  - ⬜ Two raw key literals are left in `Settings/Components/SettingInput.razor:21` and `Settings/Settings.razor.cs:39`.
  - ⬜ `"Admin"` role literals remain in `Infrastructure/DependencyInjection.cs:88`, `ApplicationDbContext.cs:45`, `Sidebar.razor` and `ToolsMenu.razor`. `RoleGroups` already uses `nameof(UserRole.Admin)`.

### 5.9 🟡 Healthcare-specific suggestions — ⬜ open
- **Soft delete** for clinical entities (Patient, Visit, MedicalRecord, Prescription, Allergy, Appointment): an `IsDeleted` flag plus an EF **global query filter**. Hard deletes and cascades lose history.
- **Optimistic concurrency.** Two staff editing the same record cause a silent last-write-wins. Add a concurrency token (`Guid Version` with `.IsConcurrencyToken()`, since SQLite has no `rowversion`) and return 409 on conflict.
- **Audit log** of who read or changed a patient's data. This is a real requirement in medical software and easy to add with the interceptor above.

### 5.10 🟡 Smaller consistency items
- ⬜ Services use explicit constructor and field boilerplate while controllers use primary constructors. Pick one style.
- ✅ `DoctorService.UpdateDoctorAsync` now names the missing entity (doctor or specialty), and the server validator checks `DepartmentId` and `SpecialtyId` first (1.6).
- ⬜ `GET /api/appointments/upcoming` is missing `///` docs and `[ProducesResponseType]`, unlike every other action.
- ⬜ `AppSettingsController.Update` duplicates the id-mismatch logic for string keys. Add an `EnsureMatching<T>` overload.
- ◐ Solution files: `Front.sln` is gone and `BeHealthy.sln` holds every project in solution folders. Consider `.slnx`. `BeHealthy.slnLaunch.user` is a user file and could be git-ignored.

---

## 6. Tests & CI

- 🔴 ⬜ **The CI workflow can't work.** `.github/workflows/docker-image.yml` triggers on `master` (the branch is `main`) and builds `BeHealthy/BeHealthy/Dockerfile`, which isn't in the repo. It also never runs `dotnet build` or `dotnet test`. Replace it with a `dotnet restore / build / test` job, and add the Docker push afterwards if you still want it.
- 🟠 ◐ **Coverage is thin, but improving.** There are now 132 tests:
  - Service tests: Appointment (conflict tests now run the real overlap predicate), Department, Doctor, Patient, Nurse (paging count only) and `SeedDataGenerator`.
  - Validator tests with `FluentValidation.TestHelper`: Appointment, Doctor, QueryParameters and a few server validators.
  - A registration guard and bUnit tests for the Blazored integration.

  Still missing:
  - **Validator tests** for Nurse, Patient, Room, Visit, Allergy, Department, Prescription, MedicalRecord, Specialty and Auth.
  - **Service tests** for the other services.
  - **Integration tests** with `WebApplicationFactory<Program>` and SQLite in-memory (`DataSource=:memory:`, keep the connection open). Real FK and cascade behaviour, real JSON, real auth. These would have caught 1.1, 1.2, 1.3 and 1.5.
  - An **authorization matrix test**: for each endpoint × role, assert the expected 200/403. This protects the `RoleGroups` work from regressions.
- 🟡 ⬜ The Tests project targets `net9.0` while the API targets `net10.0`, so you can't reference the API for `WebApplicationFactory` until they're aligned (§5.2).

---

## F. Frontend (secondary)

- 🟠 ◐ KNOWN #7: creates and updates now read the response, and `ApiClientBase` turns problem details into field errors. Still open:
  - The fire-and-forget `PostAsync`, `PutAsync` and `DeleteAsync` remain. Seven services still delete through `DeleteAsync`, so a delete the API refuses (for example a patient with clinical history, 1.3) fails silently in the UI. `PostAsync` and `PutAsync` have no callers left and can be deleted.
  - `GetAsync` still turns any `HttpRequestException` into `default`, and `GetListAsync` turns that into an empty list. A network failure looks the same as "not found" or "empty list".
- 🟠 ◐ **Duplicated code between Front and Application.**
  - ✅ The validators are shared now (`BeHealthy.Validation`, one FluentValidation version).
  - ⬜ `AppSettingsConverterHelper`, `EnumExtensions`, `DateTimeExtensions` and `ValueExtensions` still exist in both projects. Delete the Application copies (§5.1).
  - Note: Front still uses `Blazored.FluentValidation` 2.2.0, which is built against FluentValidation 11. Tests show it works with 12, but keep an eye on it when upgrading.
- 🟡 ⬜ **Two toast systems** (`ToastService` and `ToastrStateService`), both registered. `ToastService` is the one in use. `ToastrStateService` is dead and can be deleted along with its DI line.
- 🟡 ⬜ KNOWN #10: `Settings.razor` still has no `@attribute [Authorize(Roles = "Admin")]`. Consider `AuthorizeRouteView` in `Routes.razor` instead of the layout redirect.
- 🟡 ⬜ The Create and Edit pages for Doctors, Nurses and Patients (112–224 lines each) are near-copies. Extract a shared `PersonForm` component with role-specific slots, for example the specialty picker for doctors. Only `Patients/Edit` uses an extracted form, and its file name has a typo: `PatiendEditForm.razor`.

---

## Suggested order of attack

Done: validation pipeline (**§3**), paging clamps (**1.11**), the cheap high-impact bugs (**1.1–1.8**), the `IValidatorService` cleanup, one solution file.

1. **Restore `Migrate()` on startup** (KNOWN #4 regression) and update the README note.
2. Make the Front show refused deletes: switch the seven `DeleteAsync` calls to `DeleteForResponseAsync` (KNOWN #7, §F).
3. Remove the blanket `catch (Exception)` (**2.1**) and finish `GlobalExceptionHandler` (**2.3**), so bugs show up in the logs.
4. Introduce the `Result`/`ErrorType` type (**2.2**). Every later fix becomes a one-line return.
5. Security: move the JWT key out of source, close the broad endpoints to patients now that `/me` exists, add the ownership handler, lockout and rate limiting (**§4**).
6. Housekeeping: finish `Directory.Build.props`, add Central Package Management, delete dead code, fix CI (**§5.2, 5.7, §6**).
7. Bigger refactors as time allows: projections or specifications (**5.3**), account provisioning (**5.4**), auditing interceptor and soft delete (**5.8–5.9**).
