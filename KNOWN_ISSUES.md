# Known Issues & Improvement Areas

> Personal/learning project — this is not a strict audit, just a running list of things that are implemented wrong or missing so they don't get forgotten. Not everything here needs to be fixed; use judgement.

## 1. Backend has (almost) no server-side validation

This is the biggest gap. All FluentValidation validators under `BeHealthy.Application/Validations/**` are dead code:

- `IValidatorService.ValidateAsync` (`BeHealthy.Application/Services/ValidatorService.cs`) is registered in DI but never called from any Service or Controller.
- No validators are registered with the container at all (`DependencyInjection.cs` has no `AddValidatorsFromAssembly`), so even if called, `ValidateAsync` would silently no-op.
- None of the request DTOs in `BeHealthy.Shared/Dtos` have DataAnnotations, so `[ApiController]`'s automatic ModelState validation never triggers either.
- The identical validators duplicated under `BeHealthy.Front/Validations/**` are the *only* validation that ever runs, and only when going through the Blazor UI.

**Impact:** anything that talks to the API directly (curl, Postman, a different client) can write garbage data — empty required fields, negative IDs/ages, bad date ranges, references to nonexistent patients/doctors.

**Fix:** register the validators in DI and call `IValidatorService.ValidateAsync` at the start of each Application service method, before touching the DB.

## 2. Missing ownership checks (IDOR)

Most domain controllers (`AllergiesController`, `PrescriptionsController`, `MedicalRecordsController`, `AppointmentsController`, `PatientsController`, `DoctorsController`, `NursesController`) only require `[Authorize]` (any logged-in user, any role) — there's no check that the caller actually owns/can access the `patientId`/`doctorId`/`userId` in the route.

**Impact:** any authenticated user can read or modify another patient's allergies, prescriptions, medical records, appointments, or profile just by changing an ID in the URL. Destructive endpoints (delete patient/doctor/room) have no role restriction either.

**Fix:** add `[Authorize(Roles = ...)]` where appropriate, plus a check comparing the route id against the caller's claims (`ClaimTypes.NameIdentifier` or similar) before delegating to the service.

## 3. Login bypasses account lockout

`AuthService.LoginAsync` calls `UserManager.CheckPasswordAsync` directly instead of `SignInManager.CheckPasswordSignInAsync(..., lockoutOnFailure: true)`. `AccessFailedCount`/`LockoutEnd` already exist in the schema but are never incremented/checked, so there's no brute-force protection on login.

**Fix:** switch to `SignInManager.CheckPasswordSignInAsync` (or `PasswordSignInAsync`) with `lockoutOnFailure: true`.

## 4. Database is wiped on every dev startup

`BeHealthy.Infrastructure/DependencyInjection.cs` calls `context.Database.EnsureDeleted()` before `Migrate()` whenever `IsDevelopment()`. Every restart destroys all data (patients, appointments, medical records, manually created users) and recreates a hardcoded seed admin (`admin@gmail.com` / `123456aA@`).

**Fix:** drop `EnsureDeleted()` and rely on `Migrate()` alone, or gate the wipe behind an explicit opt-in flag.

## 5. No real transactional integrity for multi-step writes

`GenericRepository` calls `SaveChangesAsync()` inside every individual `Add/Update/Delete`. Multi-step operations like `PatientService.AddPatientAsync` (create Identity user → assign role → insert Patient) commit each step independently — a failure partway through can leave an orphaned Identity user with a role and no matching Patient row.

Relatedly, `UnitOfWork` doesn't actually unify anything: each repository opens its own `DbContext` from a shared factory and commits immediately, so it provides no real atomicity despite the name.

**Fix:** wrap multi-entity operations in an explicit DB transaction, or restructure `UnitOfWork`/repositories to share one `DbContext` per logical operation with a single `SaveChangesAsync()`.

## 6. Toastr notifications leak across all connected users

`ToastService` is registered as `Singleton` in `BeHealthy.Front/DependencyInjection.cs`. Every Blazor circuit subscribes to the same shared `OnShow` event, so `ShowToast` fires for *every* connected browser, not just the caller.

**Impact:** one user's "Patient created/deleted" toast pops up in every other logged-in user's session — real cross-user data leakage. There's already a correctly `Scoped` `ToastrStateService` that looks like the intended replacement; the two toast systems appear half-migrated.

**Fix:** make `ToastService` `Scoped`, or finish migrating everything to `ToastrStateService` and remove the old one.

## 7. Fire-and-forget API calls in the frontend

`Services/Api/ApiClientBase.PostAsync/PutAsync/DeleteAsync` don't check `IsSuccessStatusCode` and don't catch `HttpRequestException` (unlike their `*ForResponseAsync` siblings). Callers `await` them and then unconditionally assume success and reload the list.

**Impact:** a failed delete/update (e.g. FK constraint violation) is silently treated as success, and an unreachable API throws unhandled into the app-wide `ErrorBoundary`, blanking the whole page for that user.

**Fix:** have these helpers return a `ServiceResponse` like their siblings, and surface failures via `ToastrStateService.ShowFailed`.

## 8. Missing FK/business checks in several Application services

Unlike `AppointmentService` (which checks correctly), these insert without verifying the referenced entities exist:

- `VisitService.AddVisitAsync` — no check that `PatientId`/`DoctorId` exist.
- `PrescriptionService.AddPrescriptionAsync` — no doctor/patient existence check, no date validation.
- `MedicalRecordService` — no validation at all before writing (not even null/empty notes).
- `AllergyService.AddAllergyAsync`/`UpdateAllergyAsync` — no patient existence check.

**Fix:** mirror the existence-check pattern already used in `AppointmentService`.

## 9. Appointment double-booking race condition

`AppointmentService.CheckForConflictingAppointmentsAsync` reads existing appointments and checks overlap in memory, then inserts afterward — no DB-level unique constraint, no serializable transaction. Two concurrent booking requests for the same doctor/room/slot can both pass the check and both insert.

**Fix:** add a DB-level unique constraint/index (doctor + date + time range), or wrap check+insert in a serializable transaction.

## 10. Settings page not actually authorization-gated

`Components/Pages/Settings/Settings.razor` has no `@attribute [Authorize(Roles = "Admin")]` — it's only hidden from the nav menu via `<AuthorizeView Roles="Admin">`. Any authenticated non-admin who navigates to `/settings` directly gets the page rendered (the underlying API endpoints may still be protected, but the page itself isn't).

**Fix:** add `@attribute [Authorize(Roles = "Admin")]` to the page, and audit other role-restricted pages for the same gap.

## 11. Smaller/lower-priority items

- `GenericRepository.GetQueryable()` returns an `IQueryable` bound to an already-disposed `DbContext` (`using var context = ...` disposed before the caller enumerates). Currently unused, but a landmine for the next feature that calls it.
- Global exception handler treats everything as a generic 500 with no exception-type branching — once validation (#1) is wired up, validation errors would surface as opaque 500s instead of 400s.
- No `CancellationToken` threading anywhere in the repository/`UnitOfWork` layer.
- Blazor's auth gate is a layout-level redirect in `MainLayout.OnInitialized`, not a real `AuthorizeRouteView`/`[Authorize]` route gate — fragile if a page ever skips `MainLayout`.
- `AllergyService.UpdateAllergyAsync` fetches the existing entity only to immediately overwrite it with the DTO — harmless today since the DTO carries all fields, but fragile if the DTO is ever trimmed.
