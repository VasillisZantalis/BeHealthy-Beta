# Known Issues & Improvement Areas

> Personal/learning project — this is not a strict audit, just a running list of things that are implemented wrong or missing so they don't get forgotten. Not everything here needs to be fixed; use judgement.

## 1. ~~Backend has (almost) no server-side validation~~ (resolved)

Every request is now validated on the API before the action runs. See [`VALIDATION.md`](VALIDATION.md) for the design and how to add rules. In short:

- Shape and setting-driven rules live once in `src/BeHealthy.Validation` and run on both the Front and the API. The duplicated, never-called copies in Application and Front are gone, and so is `IValidatorService`.
- Rules that need the database (referenced ids exist, email not in use, setting value fits its type) are `*ServerValidator`s in `src/BeHealthy.Application/Validators`.
- `ValidationFilter` (registered globally) runs all of them and answers 400 `ValidationProblemDetails`.
- AppSettings-driven rules read the setting through `IValidationSettingsProvider` instead of a constructor flag, so the API enforces them too.

Not done: the setting `NurseIsRequiredForAppointment` is read by the Front and the validators but is not in the seed data, so it is always off until a row is added.

## 2. Missing ownership checks (IDOR)

Most domain controllers (`AllergiesController`, `PrescriptionsController`, `MedicalRecordsController`, `AppointmentsController`, `PatientsController`, `DoctorsController`, `NursesController`) only require `[Authorize]` (any logged-in user, any role) — there's no check that the caller actually owns/can access the `patientId`/`doctorId`/`userId` in the route.

**Impact:** any authenticated user can read or modify another patient's allergies, prescriptions, medical records, appointments, or profile just by changing an ID in the URL. Destructive endpoints (delete patient/doctor/room) have no role restriction either.

**Fix:** add `[Authorize(Roles = ...)]` where appropriate, plus a check comparing the route id against the caller's claims (`ClaimTypes.NameIdentifier` or similar) before delegating to the service.

## 3. Login bypasses account lockout

`AuthService.LoginAsync` calls `UserManager.CheckPasswordAsync` directly instead of `SignInManager.CheckPasswordSignInAsync(..., lockoutOnFailure: true)`. `AccessFailedCount`/`LockoutEnd` already exist in the schema but are never incremented/checked, so there's no brute-force protection on login.

**Fix:** switch to `SignInManager.CheckPasswordSignInAsync` (or `PasswordSignInAsync`) with `lockoutOnFailure: true`.

## 4. Database is wiped on every dev startup

`src/BeHealthy.Infrastructure/DependencyInjection.cs` calls `context.Database.EnsureDeleted()` before `Migrate()` whenever `IsDevelopment()`. Every restart destroys all data (patients, appointments, medical records, manually created users) and recreates a hardcoded seed admin (`admin@gmail.com` / `123456aA@`).

**Fix:** drop `EnsureDeleted()` and rely on `Migrate()` alone, or gate the wipe behind an explicit opt-in flag.

## 5. ~~No real transactional integrity for multi-step writes~~ (resolved)

The custom `UnitOfWork` was removed; EF Core's request-scoped `ApplicationDbContext` is the unit of work. Rules for writes:

- Repositories only stage changes (`AddAsync`/`UpdateAsync`/`DeleteAsync`); they never write to the DB on their own (no `ExecuteUpdate`/`ExecuteDelete`).
- A service ends a write with a single `SaveChangesAsync()` on a repository. All repositories (and Identity's `UserManager`) share the same `DbContext`, and EF wraps one save in a transaction, so that is atomic.
- When an operation saves more than once — anything calling `UserManager` (which saves itself) plus our own entities — wrap it in `ITransactionManager.ExecuteInTransactionAsync`. It commits only when the operation returns a successful `ServiceResponse`; a failed response or an exception rolls back and clears the change tracker (exceptions are rethrown to the global handler).
- Do all lookups/validation before the transaction, so it only contains the writes.
- Every async method takes a `CancellationToken` (controllers get it bound to `HttpContext.RequestAborted`) and must forward it; `.editorconfig` raises CA2016 as a warning when a call doesn't. A cancelled request inside a transaction is rolled back like any other exception.

## 6. Toastr notifications leak across all connected users

`ToastService` is registered as `Singleton` in `src/BeHealthy.Front/DependencyInjection.cs`. Every Blazor circuit subscribes to the same shared `OnShow` event, so `ShowToast` fires for *every* connected browser, not just the caller.

**Impact:** one user's "Patient created/deleted" toast pops up in every other logged-in user's session — real cross-user data leakage. There's already a correctly `Scoped` `ToastrStateService` that looks like the intended replacement; the two toast systems appear half-migrated.

**Fix:** make `ToastService` `Scoped`, or finish migrating everything to `ToastrStateService` and remove the old one.

## 7. Fire-and-forget API calls in the frontend

`Services/Api/ApiClientBase.PostAsync/PutAsync/DeleteAsync` don't check `IsSuccessStatusCode` and don't catch `HttpRequestException` (unlike their `*ForResponseAsync` siblings). Callers `await` them and then unconditionally assume success and reload the list.

**Impact:** a failed delete/update (e.g. FK constraint violation) is silently treated as success, and an unreachable API throws unhandled into the app-wide `ErrorBoundary`, blanking the whole page for that user.

**Fix:** have these helpers return a `ServiceResponse` like their siblings, and surface failures via `ToastrStateService.ShowFailed`.

**Status:** partly fixed. Room, specialty, medical record and setting writes now use the `*ForResponseAsync` helpers, so a rejected save shows its message. The remaining `PostAsync`/`PutAsync`/`DeleteAsync` callers are the delete buttons.

## 8. ~~Missing FK/business checks in several Application services~~ (resolved)

The existence checks for visits, prescriptions, medical records, allergies (and every other request with a foreign key) now run as `*ServerValidator`s on the API (#1). Code that calls the services directly, such as `SeedingService`, skips them, which is fine for trusted callers.

## 9. Appointment double-booking race condition

`AppointmentService.CheckForConflictingAppointmentsAsync` reads existing appointments and checks overlap in memory, then inserts afterward — no DB-level unique constraint, no serializable transaction. Two concurrent booking requests for the same doctor/room/slot can both pass the check and both insert.

**Fix:** add a DB-level unique constraint/index (doctor + date + time range), or wrap check+insert in a serializable transaction.

## 10. Settings page not actually authorization-gated

`Components/Pages/Settings/Settings.razor` has no `@attribute [Authorize(Roles = "Admin")]` — it's only hidden from the nav menu via `<AuthorizeView Roles="Admin">`. Any authenticated non-admin who navigates to `/settings` directly gets the page rendered (the underlying API endpoints may still be protected, but the page itself isn't).

**Fix:** add `@attribute [Authorize(Roles = "Admin")]` to the page, and audit other role-restricted pages for the same gap.

## 11. Smaller/lower-priority items

- Global exception handler treats everything except a `FluentValidation.ValidationException` (now a 400, see #1) as a generic 500. `DbUpdateException`/`DbUpdateConcurrencyException` still surface as 500s instead of 409s.
- Blazor's auth gate is a layout-level redirect in `MainLayout.OnInitialized`, not a real `AuthorizeRouteView`/`[Authorize]` route gate — fragile if a page ever skips `MainLayout`.
- `AllergyService.UpdateAllergyAsync` fetches the existing entity only to immediately overwrite it with the DTO — harmless today since the DTO carries all fields, but fragile if the DTO is ever trimmed.
