# BeHealthy API — Architecture Roadmap & Specification

> **Document owner:** Development Team  
> **Branch:** `feature/api-separation`  
> **Status:** In Progress  
> **Last updated:** 2026-07-14

---

## Table of Contents

1. [Why We Are Doing This](#1-why-we-are-doing-this)
2. [Target Architecture](#2-target-architecture)
3. [Project Setup — BeHealthy.Api](#3-project-setup--behealthyapi)
4. [Changes to the Existing Blazor Project](#4-changes-to-the-existing-blazor-project)
5. [Authentication & Authorization Strategy](#5-authentication--authorization-strategy)
6. [API Contract — All Controllers & Endpoints](#6-api-contract--all-controllers--endpoints)
7. [DTOs & Shared Models](#7-dtos--shared-models)
8. [Migration Phases & Checklist](#8-migration-phases--checklist)
9. [Definition of Done](#9-definition-of-done)
10. [Technical Decisions & Constraints](#10-technical-decisions--constraints)

---

## 1. Why We Are Doing This

### Current Problem

The Blazor frontend currently calls application services **directly**. There is no HTTP boundary between the UI and the business logic. This means:

- The frontend and the business logic are **tightly coupled** — you cannot change one without touching the other.
- No other client (mobile app, third-party integration, admin dashboard, reporting tool) can consume the system's data without copy-pasting the entire service layer.
- There is no enforced API contract — the UI and the data layer can drift silently.
- Testing is harder because you cannot test business logic independently of a running Blazor server.

### What We Want

A clear HTTP boundary where:

- **BeHealthy.Api** owns all business logic, data access, and domain rules.
- **BeHealthy (Blazor)** is a pure UI layer that knows nothing about databases or services — it only speaks HTTP.
- Any future client (mobile, reporting, third-party) calls the same API.
- The API is self-documented (Swagger/OpenAPI) and independently testable.

---

## 2. Target Architecture

### Solution Structure (after migration)

```
BeHealthy.sln
│
├── BeHealthy.Domain          # Entities, enums, value objects — no changes needed
├── BeHealthy.Application     # Service interfaces, DTOs, UnitOfWork interface — no changes needed
├── BeHealthy.Infrastructure  # EF Core, repositories, UnitOfWork impl — no changes needed
├── BeHealthy.Shared          # Query parameters, localization resources — minor additions
│
├── BeHealthy.Api             # NEW: ASP.NET Core Web API
│   ├── Controllers/          # One controller per domain area
│   ├── Middleware/           # Error handling, auth middleware
│   ├── Extensions/           # Program.cs extension methods
│   └── Program.cs
│
└── BeHealthy               # Blazor Server — becomes a pure UI shell
    ├── Components/         # All existing .razor files stay here
    ├── HttpClients/        # NEW: typed HttpClient wrappers replacing service injections
    └── Program.cs          # Stripped of service/infrastructure registrations
```

### Data Flow (after migration)

```
[Browser]
    ↓ HTTP (renders Blazor interactive components)
[BeHealthy — Blazor Server]
    ↓ HTTP/JSON  (HttpClient calls)
[BeHealthy.Api — ASP.NET Core Web API]
    ↓ C# method calls
[BeHealthy.Application — Services]
    ↓ C# method calls
[BeHealthy.Infrastructure — Repositories / EF Core]
    ↓ SQL
[SQLite Database]
```

---

## 3. Project Setup — BeHealthy.Api

### 3.1 Create the Project

```bash
cd BeHealthy
dotnet new webapi -n BeHealthy.Api --no-openapi false
dotnet sln add BeHealthy.Api/BeHealthy.Api.csproj
```

### 3.2 Project References

The Api project must reference Application and Infrastructure — it does NOT reference the Blazor project:

```xml
<!-- BeHealthy.Api.csproj -->
<ItemGroup>
  <ProjectReference Include="..\BeHealthy.Application\BeHealthy.Application.csproj" />
  <ProjectReference Include="..\BeHealthy.Infrastructure\BeHealthy.Infrastructure.csproj" />
  <ProjectReference Include="..\BeHealthy.Shared\BeHealthy.Shared.csproj" />
</ItemGroup>
```

### 3.3 NuGet Packages Needed

| Package | Reason |
|---|---|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | JWT auth for the API |
| `Swashbuckle.AspNetCore` | Swagger / OpenAPI docs |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | Needed by Infrastructure |

### 3.4 Program.cs — What to Register

```csharp
// BeHealthy.Api/Program.cs

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddApplication();       // same extension as today
builder.Services.AddInfrastructure(builder.Configuration);  // same extension as today

// Auth — see Section 5
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* JWT settings */ });

builder.Services.AddAuthorization(options => {
    // role-based policies matching existing UserRole enum
    options.AddPolicy("AdminOnly",  p => p.RequireRole("Admin"));
    options.AddPolicy("DoctorOrAdmin", p => p.RequireRole("Admin", "Doctor"));
    options.AddPolicy("StaffOrAbove", p => p.RequireRole("Admin", "Staff", "Doctor", "Nurse"));
});

// CORS — allow the Blazor frontend origin
builder.Services.AddCors(options => {
    options.AddPolicy("BlazorClient", policy =>
        policy.WithOrigins("https://localhost:PORT_OF_BLAZOR_APP")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```

### 3.5 appsettings.json — What to Add

```json
{
  "ConnectionStrings": {
    "Default": "Data Source=../BeHealthy/BeHealthy/behealthy.db"
  },
  "Jwt": {
    "Key": "your-secret-key-at-least-256-bits",
    "Issuer": "BeHealthy.Api",
    "Audience": "BeHealthy.Client",
    "ExpiryMinutes": 60
  }
}
```

> **Note on the database path:** Both the API and the Blazor project share the same SQLite file for now. The relative path above points to the existing DB. In a future deployment, the API owns the DB and the Blazor project has no DB path at all.

---

## 4. Changes to the Existing Blazor Project

These changes happen **incrementally** as each controller is built (see Phase plan in Section 8).

### 4.1 What Gets Removed from Blazor's Program.cs

```csharp
// REMOVE these — the API now owns this:
builder.Services.AddApplication();
builder.Services.AddInfrastructure(config);
```

The Blazor project will keep only UI-state services:
- `ModalStateService`
- `NavMenuState`
- `LoaderServiceState`
- `BreadcrumbServiceState`
- `AlertModalStateService`
- `ToastService`
- `IModalService`
- `AuthenticationStateProvider`

### 4.2 What Replaces Service Injections in Components

Every component today does:
```csharp
[Inject] IDoctorService DoctorService { get; set; }
```

After migration, each component injects a typed HTTP client instead:
```csharp
[Inject] DoctorApiClient DoctorClient { get; set; }
```

The typed client lives in `BeHealthy/HttpClients/` and wraps `HttpClient` calls. Example:

```csharp
// BeHealthy/HttpClients/DoctorApiClient.cs
public class DoctorApiClient(HttpClient http)
{
    public Task<PaginatedResult<DoctorDto>?> GetAllAsync(DoctorQueryParameters p)
        => http.GetFromJsonAsync<PaginatedResult<DoctorDto>>($"api/doctors?page={p.PageNumber}&pageSize={p.PageSize}");

    public Task<DoctorDto?> GetByIdAsync(int id)
        => http.GetFromJsonAsync<DoctorDto>($"api/doctors/{id}");

    public async Task<ServiceResponse> CreateAsync(DoctorCreateDto dto)
    {
        var res = await http.PostAsJsonAsync("api/doctors", dto);
        return await res.Content.ReadFromJsonAsync<ServiceResponse>() ?? ServiceResponse.Failed("Unknown error");
    }
    // ... etc
}
```

Typed clients are registered in Blazor's `Program.cs`:
```csharp
builder.Services.AddHttpClient<DoctorApiClient>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!));
```

### 4.3 Authentication in the Blazor Project

The existing cookie-based auth (`auth_cookie`) is used for Blazor's own Identity pages (`/login`, `/register`, etc.). This **stays exactly as it is** — those pages are Blazor-internal.

For calls to the API, the Blazor app will need to pass the authenticated user's identity. The recommended approach is a shared auth cookie (both apps run on the same domain / under a reverse proxy), or a JWT token stored in memory after login. **This is a decision to make at Phase 2** — do not change auth until Phase 1 (scaffold + first controller) is proven working.

---

## 5. Authentication & Authorization Strategy

### 5.1 Roles (from existing `UserRole` enum)

| Role | Value | Access |
|---|---|---|
| Admin | 0 | Full access to everything |
| Staff | 1 | Read all, limited write |
| Doctor | 2 | Own patients, own appointments, prescriptions |
| Nurse | 3 | Assigned appointments, patient read |
| Patient | 4 | Own profile, own appointments, own medical records |

### 5.2 Per-Endpoint Authorization Rules

The table below defines the access rule for each resource. These map to `[Authorize(Policy = "...")]` or `[Authorize(Roles = "...")]` attributes on controllers.

| Resource | GET (list) | GET (by id) | POST | PUT | DELETE |
|---|---|---|---|---|---|
| Patients | Admin, Staff, Doctor, Nurse | Admin, Staff, Doctor, Nurse, own Patient | Admin, Staff | Admin, Staff | Admin |
| Doctors | All authenticated | All authenticated | Admin | Admin, own Doctor | Admin |
| Nurses | Admin, Staff | Admin, Staff, Doctor | Admin | Admin, own Nurse | Admin |
| Appointments | Admin, Staff (all); Doctor/Nurse/Patient (own) | Same scoping | Admin, Staff, Doctor | Admin, Staff, Doctor | Admin, Staff |
| Departments | All authenticated | All authenticated | Admin | Admin | Admin |
| Rooms | All authenticated | All authenticated | Admin | Admin | Admin |
| Specialties | All authenticated | All authenticated | Admin | Admin | Admin |
| Medical Records | Admin, Doctor (own patients), own Patient | Same | Admin, Doctor | Admin, Doctor | Admin |
| Prescriptions | Admin, Doctor, own Patient | Same | Admin, Doctor | Admin, Doctor | Admin |
| Visits | Admin, Doctor, own Patient | Same | Admin, Doctor | Admin, Doctor | Admin |
| Allergies | Admin, Doctor, own Patient | Same | Admin, Doctor, own Patient | Admin, Doctor, own Patient | Admin, Doctor |
| App Settings | Admin | Admin | Admin | Admin | — |
| Seeding | Admin | Admin | Admin | — | — |

---

## 6. API Contract — All Controllers & Endpoints

Base path: `/api/`  
Response format: `application/json`  
All list endpoints return `PaginatedResult<T>` unless noted.

---

### 6.1 Auth — `POST /api/auth/login`

> Handles login and token issuance. The existing Identity Razor pages (`/login`, `/register`) stay in the Blazor project for browser-based login. This endpoint is for programmatic API access (future mobile/third-party clients).

| Method | Route | Description | Auth |
|---|---|---|---|
| POST | `/api/auth/login` | Accept `LoginDto`, return JWT token | Anonymous |
| POST | `/api/auth/refresh` | Refresh JWT using refresh token | Anonymous |

**Request body** (`POST /api/auth/login`):
```json
{ "username": "admin@gmail.com", "password": "123456aA@" }
```

**Response:**
```json
{
  "token": "eyJ...",
  "expiresAt": "2026-07-14T12:00:00Z",
  "user": { "id": "...", "username": "admin@gmail.com", "role": "Admin" }
}
```

---

### 6.2 Patients — `PatientsController`

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/patients` | `GetAllPatientsAsync(params)` | Paginated list with filters |
| GET | `/api/patients/simple` | `GetAllPatientsSimpleAsync()` | Lightweight list for dropdowns |
| GET | `/api/patients/{id}` | `GetPatientByIdAsync(id)` | Single patient detail |
| GET | `/api/patients/count` | `GetPatientCountAsync()` | Total count (used by dashboard) |
| GET | `/api/patients/profile` | `GetPatientProfileByUserIdAsync(userId)` | Profile for logged-in patient |
| GET | `/api/patients/{id}/appointments` | `GetPatientAppointmentsByUserIdAsync` | Patient's own appointments |
| GET | `/api/patients/{id}/doctors` | `GetMyDoctorsAsync(userId)` | Doctors assigned to a patient |
| POST | `/api/patients` | `AddPatientAsync(dto)` | Create new patient |
| PUT | `/api/patients/{id}` | `UpdatePatientAsync(dto)` | Update patient |
| DELETE | `/api/patients/{id}` | `DeletePatientAsync(id)` | Delete patient |

**Query parameters for GET `/api/patients`:**
- `pageNumber` (int, default 1)
- `pageSize` (int, default 10)
- `searchTerm` (string)
- `firstName` (string)
- `lastName` (string)
- `orderBy` (string)
- `orderDescending` (bool)

---

### 6.3 Doctors — `DoctorsController`

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/doctors` | `GetAllDoctorsAsync(params)` | Paginated list with filters |
| GET | `/api/doctors/simple` | `GetAllDoctorsSimpleAsync()` | Lightweight list for dropdowns |
| GET | `/api/doctors/{id}` | `GetDoctorByIdAsync(id)` | Single doctor detail |
| GET | `/api/doctors/count` | `GetDoctorCountAsync()` | Total count (dashboard) |
| GET | `/api/doctors/profile` | `GetDoctorProfileByUserIdAsync(userId)` | Profile for logged-in doctor |
| GET | `/api/doctors/{id}/appointments` | `GetDoctorAppointmentsByUserIdAsync` | Doctor's appointments |
| GET | `/api/doctors/{id}/patients` | `GetMyPatientsAsync(userId)` | Patients under a doctor |
| POST | `/api/doctors` | `AddDoctorAsync(dto)` | Create new doctor + user account |
| PUT | `/api/doctors/{id}` | `UpdateDoctorAsync(dto)` | Update doctor |
| DELETE | `/api/doctors/{id}` | `DeleteDoctorAsync(id)` | Delete doctor |

**Query parameters for GET `/api/doctors`:**
- `pageNumber`, `pageSize`, `searchTerm`, `orderBy`, `orderDescending`
- `specialtyId` (int, filter by specialty)

---

### 6.4 Nurses — `NursesController`

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/nurses` | `GetAllNursesAsync(params)` | Paginated list |
| GET | `/api/nurses/simple` | `GetAllNursesSimpleAsync()` | Lightweight list for dropdowns |
| GET | `/api/nurses/{id}` | `GetNurseByIdAsync(id)` | Single nurse detail |
| GET | `/api/nurses/count` | `GetNurseCountAsync()` | Total count (dashboard) |
| GET | `/api/nurses/profile` | `GetNurseProfileByUserIdAsync(userId)` | Profile for logged-in nurse |
| GET | `/api/nurses/by-patient` | `GetNursesOfPatientByUserId(userId)` | Nurses for a patient |
| POST | `/api/nurses` | `AddNurseAsync(dto)` | Create new nurse + user account |
| PUT | `/api/nurses/{id}` | `UpdateNurseAsync(dto)` | Update nurse |
| DELETE | `/api/nurses/{id}` | `DeleteNurseAsync(id)` | Delete nurse |

---

### 6.5 Appointments — `AppointmentsController`

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/appointments` | `GetAllAppointmentsAsync(params)` | Paginated list, filterable |
| GET | `/api/appointments/{id}` | `GetAppointmentByIdAsync(id)` | Single appointment |
| GET | `/api/appointments/reasons` | `GetAppointmentReasonCounts()` | Reason distribution (dashboard chart) |
| GET | `/api/appointments/by-doctor/{doctorId}` | `GetAllAppointmentsByDoctorIdAsync` | All for a doctor |
| GET | `/api/appointments/by-patient/{patientId}` | `GetAllAppointmentsByPatientIdAsync` | All for a patient |
| GET | `/api/appointments/by-user` | `GetAllAppointmentsByUserIdAsync` | Appointments by current user's ID |
| POST | `/api/appointments` | `AddAppointmentAsync(dto)` | Create appointment |
| PUT | `/api/appointments/{id}` | `UpdateAppointmentAsync(dto)` | Update appointment |
| DELETE | `/api/appointments/{id}` | `DeleteAppointmentAsync(id)` | Delete appointment |

**Query parameters for GET `/api/appointments`:**
- `pageNumber`, `pageSize`, `searchTerm`, `orderBy`, `orderDescending`
- `doctorId` (int), `patientId` (int)

---

### 6.6 Departments — `DepartmentsController`

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/departments` | `GetAllDepartmentsAsync()` | All departments |
| GET | `/api/departments/{id}` | `GetDepartmentByIdAsync(id)` | Department with full nav data (doctors, nurses, patients, rooms) |
| POST | `/api/departments` | `AddDepartmentAsync(dto)` | Create department |
| PUT | `/api/departments/{id}` | `UpdateDepartmentAsync(dto)` | Update department |
| DELETE | `/api/departments/{id}` | `DeleteDepartmentAsync(id)` | Delete department |

---

### 6.7 Rooms — `RoomsController`

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/rooms` | `GetAllRoomsAsync()` | All rooms |
| GET | `/api/rooms/{id}` | `GetRoomByIdAsync(id)` | Single room |
| POST | `/api/rooms` | `AddRoomAsync(dto)` | Create room |
| PUT | `/api/rooms/{id}` | `UpdateRoomAsync(dto)` | Update room |
| DELETE | `/api/rooms/{id}` | `DeleteRoomAsync(id)` | Delete room |

---

### 6.8 Specialties — `SpecialtiesController`

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/specialties` | `GetSpecialtiesAsync()` | All specialties |
| GET | `/api/specialties/{id}` | `GetSpecialtyByIdAsync(id)` | Single specialty |
| POST | `/api/specialties` | `AddSpecialtyAsync(dto)` | Create specialty |
| PUT | `/api/specialties/{id}` | `UpdateSpecialtyAsync(dto)` | Update specialty |
| DELETE | `/api/specialties/{id}` | `DeleteSpecialtyAsync(id)` | Delete specialty |

---

### 6.9 Medical Records — `MedicalRecordsController`

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/medical-records` | `GetAllMedicalRecordsAsync()` | All records (admin) |
| GET | `/api/medical-records/{id}` | `GetMedicalRecordByIdAsync(id)` | Single record |
| GET | `/api/medical-records/by-patient/{patientId}` | `GetMedicalRecordsByPatientIdAsync` | Records for a patient |
| POST | `/api/medical-records` | `AddMedicalRecordAsync(dto)` | Create record |
| PUT | `/api/medical-records/{id}` | `UpdateMedicalRecordAsync(dto)` | Full update |
| PATCH | `/api/medical-records/{id}/notes` | `UpdateMedicalRecordNotesAsync` | Update notes only |
| DELETE | `/api/medical-records/{id}` | `DeleteMedicalRecordAsync(id)` | Delete record |

---

### 6.10 Visits — `VisitsController`

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/visits` | `GetAllVisitsAsync()` | All visits |
| GET | `/api/visits/{id}` | `GetVisitWithDetailsAsync(id)` | Visit with diagnoses, treatments, lab results |
| GET | `/api/visits/by-patient/{patientId}` | `GetVisitsByPatientIdAsync` | Visits for a patient |
| GET | `/api/visits/{id}/diagnoses` | `GetDiagnosesByVisitIdAsync` | Diagnoses for a visit |
| GET | `/api/visits/{id}/treatments` | `GetTreatmentsByVisitIdAsync` | Treatments for a visit |
| GET | `/api/visits/{id}/lab-results` | `GetLabResultsByVisitIdAsync` | Lab results for a visit |
| POST | `/api/visits` | `AddVisitAsync(dto)` | Create visit |
| PUT | `/api/visits/{id}` | `UpdateVisitAsync(dto)` | Update visit |
| DELETE | `/api/visits/{id}` | `DeleteVisitAsync(id)` | Delete visit |

---

### 6.11 Prescriptions — `PrescriptionsController`

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/prescriptions` | `GetAllPrescriptionsAsync()` | All prescriptions |
| GET | `/api/prescriptions/{id}` | `GetPrescriptionByIdAsync(id)` | Single prescription |
| GET | `/api/prescriptions/by-patient/{id}` | `GetPrescriptionsByPatientIdAsync` | Patient's prescriptions |
| POST | `/api/prescriptions` | `AddPrescriptionAsync(dto)` | Create prescription |
| PUT | `/api/prescriptions/{id}` | `UpdatePrescriptionAsync(dto)` | Update prescription |
| DELETE | `/api/prescriptions/{id}` | `DeletePrescriptionAsync(id)` | Delete prescription |

---

### 6.12 Allergies — `AllergiesController`

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/allergies/by-patient/{patientId}` | `GetAllergiesByPatientIdAsync` | Patient's allergies |
| GET | `/api/allergies/{id}` | `GetAllergyByIdAsync(id)` | Single allergy |
| POST | `/api/allergies` | `AddAllergyAsync(dto)` | Add allergy |
| PUT | `/api/allergies/{id}` | `UpdateAllergyAsync(dto)` | Update allergy |
| DELETE | `/api/allergies/{id}` | `DeleteAllergyAsync(id)` | Delete allergy |

---

### 6.13 App Settings — `AppSettingsController`

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/settings` | `GetAppSettingsAsync()` | All settings |
| GET | `/api/settings/{key}` | `GetSettingByKeyAsync(key)` | Single setting by key |
| POST | `/api/settings/bulk` | `GetMassAppSettingsAsync(keys)` | Multiple settings by key list |
| PUT | `/api/settings/{key}` | `UpdateSettingAsync(setting)` | Update a setting |

---

### 6.14 Seeding — `SeedingController`

> Admin-only. Used by the Seeding modal in the UI.

| Method | Route | Service Method | Description |
|---|---|---|---|
| GET | `/api/seeding/counts` | `CheckEntityCountsAsync()` | Returns `Dictionary<string, int>` |
| GET | `/api/seeding/needs-seeding` | `NeedsSeedingAsync()` | Returns bool |
| POST | `/api/seeding/doctors` | `SeedDoctorsAsync(count)` | Seed N doctors |
| POST | `/api/seeding/patients` | `SeedPatientsAsync(count)` | Seed N patients |
| POST | `/api/seeding/nurses` | `SeedNursesAsync(count)` | Seed N nurses |
| POST | `/api/seeding/appointments` | `SeedAppointmentsAsync(count)` | Seed N appointments |
| POST | `/api/seeding/all` | `SeedAllAsync(options)` | Seed everything at once |

---

### 6.15 Dashboard — `DashboardController`

> Aggregates data for the dashboard widgets. Avoids multiple roundtrips from the frontend.

| Method | Route | Description |
|---|---|---|
| GET | `/api/dashboard/summary` | Returns `{ patientCount, doctorCount, nurseCount, appointmentReasonCounts, usersInRolesCount }` |

This is the only controller that does NOT directly map to a single service — it calls `IPatientService.GetPatientCountAsync()`, `IDoctorService.GetDoctorCountAsync()`, `INurseService.GetNurseCountAsync()`, `IAppointmentService.GetAppointmentReasonCounts()`, and `IUserService.GetUsersInRolesCount()` and returns them as one response object.

---

## 7. DTOs & Shared Models

### 7.1 Where DTOs Live (Current vs Target)

| DTO Category | Current Location | After Migration |
|---|---|---|
| All domain DTOs (`PatientDto`, `DoctorDto`, etc.) | `BeHealthy.Application\Dtos\` | Stay in `BeHealthy.Application\Dtos\` — both API and Blazor reference Application |
| Query parameters (`QueryParameters`, `PatientQueryParameters`, etc.) | `BeHealthy.Shared\Parameters\` | Stay in `BeHealthy.Shared\Parameters\` |
| Localization resources | `BeHealthy.Shared\Locales\` | Stay in `BeHealthy.Shared\Locales\` |
| `ServiceResponse`, `PaginatedResult<T>` | `BeHealthy.Application\Dtos\` | Stay — these cross the HTTP boundary |

> **No DTO files need to move.** The Blazor project already references `BeHealthy.Application` for its service interfaces; it will continue to reference it for DTOs when calling the API. The API project references `BeHealthy.Application` too.

### 7.2 New DTO Needed: `DashboardSummaryDto`

Create in `BeHealthy.Application\Dtos\Dashboard\DashboardSummaryDto.cs`:

```csharp
public class DashboardSummaryDto
{
    public int PatientCount { get; set; }
    public int DoctorCount { get; set; }
    public int NurseCount { get; set; }
    public Dictionary<AppointmentReason, int> AppointmentReasonCounts { get; set; } = [];
    public Dictionary<string, int> UsersInRolesCount { get; set; } = [];
}
```

### 7.3 Enums — Serialization

The API must serialize enums as strings (not integers) so the Blazor frontend can consume them without mapping:

```csharp
builder.Services.AddControllers()
    .AddJsonOptions(o => {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
```

---

## 8. Migration Phases & Checklist

### Phase 0 — Project Scaffolding

> Goal: Get the API project running with no business logic yet. The Blazor app is unchanged.

- [ ] Create `BeHealthy.Api` project (`dotnet new webapi`)
- [ ] Add to solution (`dotnet sln add`)
- [ ] Add project references (Application, Infrastructure, Shared)
- [ ] Configure `Program.cs` (services, CORS, Swagger, JSON options)
- [ ] Set up `appsettings.json` with connection string and JWT config
- [ ] Verify API starts and Swagger UI loads at `/swagger`
- [ ] Add global error-handling middleware (`UseExceptionHandler`)
- [ ] Commit: `scaffold: add BeHealthy.Api project`

---

### Phase 1 — Reference Data Controllers

> Goal: Controllers for data that doesn't change often and has no auth complexity. Safe to start here to prove the pattern.

**Scope:** Specialties, Departments, Rooms

- [ ] `SpecialtiesController` — all 5 endpoints
- [ ] `DepartmentsController` — all 5 endpoints
- [ ] `RoomsController` — all 5 endpoints
- [ ] Test each endpoint via Swagger
- [ ] Commit: `feat(api): add specialties, departments, rooms controllers`

**No Blazor changes yet in this phase.**

---

### Phase 2 — Staff Controllers (Doctors & Nurses)

> Goal: The two most-used staff resources.

- [ ] `DoctorsController` — all 10 endpoints
- [ ] `NursesController` — all 9 endpoints
- [ ] Test paginated list, create, update, delete via Swagger
- [ ] **Blazor change:** Update `Pages/Doctors/Doctors.razor`, `Create.razor`, `Edit.razor` to use `DoctorApiClient` instead of `IDoctorService`
- [ ] **Blazor change:** Update `Pages/Nurses/Nurses.razor`, `Create.razor`, `Edit.razor` to use `NurseApiClient`
- [ ] Remove `IDoctorService` and `INurseService` injections from updated components
- [ ] Commit: `feat(api): doctors and nurses controllers + blazor client updates`

---

### Phase 3 — Patient Controller

> Goal: The core entity of the system.

- [ ] `PatientsController` — all 10 endpoints
- [ ] **Blazor change:** Update `Pages/Patients/Patients.razor`, `Create.razor`, `Edit.razor`
- [ ] Update patient sub-components: `AllergiesGrid`, `AllergiesModal`, `MedicalRecords`, `Prescriptions`
- [ ] Commit: `feat(api): patients controller + blazor client updates`

---

### Phase 4 — Appointments Controller

- [ ] `AppointmentsController` — all 9 endpoints
- [ ] **Blazor change:** Update `Pages/Appointments/Appointments.razor` and `AppointmentModal`
- [ ] Update Dashboard `UpcomingAppointments` and `ReasonOfAppointmentsChart` widgets
- [ ] Commit: `feat(api): appointments controller + blazor client updates`

---

### Phase 5 — Clinical Data (Medical Records, Visits, Prescriptions, Allergies)

- [ ] `MedicalRecordsController` — all 7 endpoints
- [ ] `VisitsController` — all 9 endpoints
- [ ] `PrescriptionsController` — all 6 endpoints
- [ ] `AllergiesController` — all 5 endpoints
- [ ] **Blazor change:** Update `Pages/MedicalRecords/MedicalRecord.razor`, `Pages/Visits/Visit.razor`
- [ ] Update all modal components that manage prescriptions, visits, diagnoses
- [ ] Commit: `feat(api): clinical data controllers + blazor client updates`

---

### Phase 6 — Dashboard & Settings

- [ ] `DashboardController` — summary endpoint
- [ ] `AppSettingsController` — all 4 endpoints
- [ ] **Blazor change:** Update `Pages/Dashboard/Home.razor` dashboard widgets to use one `DashboardApiClient` call
- [ ] **Blazor change:** Update `Pages/Settings/Settings.razor` to use `AppSettingsApiClient`
- [ ] Commit: `feat(api): dashboard and settings controllers + blazor client updates`

---

### Phase 7 — Seeding & Admin Tools

- [ ] `SeedingController` — all 7 endpoints
- [ ] **Blazor change:** Update `SeedingModal` component to use `SeedingApiClient`
- [ ] Commit: `feat(api): seeding controller + blazor client update`

---

### Phase 8 — Cleanup & Decoupling

> Goal: The Blazor project no longer depends on Application or Infrastructure layers.

- [ ] Remove `builder.Services.AddApplication()` from Blazor's `Program.cs`
- [ ] Remove `builder.Services.AddInfrastructure()` from Blazor's `Program.cs`
- [ ] Remove project references to `BeHealthy.Application` and `BeHealthy.Infrastructure` from the Blazor `.csproj`
- [ ] Verify Blazor project still compiles (it should reference only `BeHealthy.Shared` for DTOs and query params — **this requires moving DTOs to Shared or a new `BeHealthy.Contracts` project if they are needed by the Blazor client**)
- [ ] Run full regression test of all pages
- [ ] Commit: `refactor: decouple blazor from application and infrastructure layers`

---

### Phase 9 — Authentication Hardening (Optional but Recommended)

- [ ] Decide on auth strategy: shared cookie vs. JWT stored client-side
- [ ] If JWT: update `DoctorApiClient` and all typed clients to attach `Authorization: Bearer {token}` header
- [ ] Add refresh token logic if needed
- [ ] Add `[Authorize]` attributes to all controllers per the role table in Section 5
- [ ] Commit: `feat(api): add authorization to all controllers`

---

## 9. Definition of Done

A controller is considered **done** when:

1. All endpoints listed in Section 6 for that controller exist and return correct HTTP status codes:
   - `200 OK` — successful GET or PUT
   - `201 Created` — successful POST (with `Location` header pointing to the new resource)
   - `204 No Content` — successful DELETE
   - `400 Bad Request` — validation failure (model state invalid)
   - `401 Unauthorized` — not authenticated
   - `403 Forbidden` — authenticated but wrong role
   - `404 Not Found` — resource doesn't exist
2. The corresponding Blazor component(s) have been updated to call the API endpoint instead of injecting the service directly.
3. The direct service injection (`[Inject] IXxxService`) has been removed from the updated component.
4. All existing pages related to that resource still work end-to-end in the browser.
5. Swagger shows the endpoint with correct request/response models.

---

## 10. Technical Decisions & Constraints

### 10.1 Shared Database (for now)

Both the Blazor app and the API project point to the **same SQLite file** during development. This is intentional for the migration period — it avoids data sync problems while the architecture is being split.

Once Phase 8 (cleanup) is complete, only the API project should have a database connection string. The Blazor project config can drop the `ConnectionStrings` section entirely.

### 10.2 Do Not Rename or Move Existing Service Interfaces

`IPatientService`, `IDoctorService`, etc. must not be renamed or moved. The API controllers call them directly. The Blazor app will stop calling them only after the typed HTTP clients replace them in each component.

### 10.3 Typed HTTP Clients Over Raw HttpClient

Never inject `HttpClient` directly into a Blazor component. Always create a typed client class in `BeHealthy/HttpClients/`. This mirrors the service abstraction pattern already in the codebase and keeps components thin.

### 10.4 Error Handling — Global Middleware

The API must have a global `ExceptionHandlerMiddleware` that catches unhandled exceptions and returns:

```json
{
  "success": false,
  "errorMessage": "An unexpected error occurred."
}
```

This reuses the existing `ServiceResponse` shape so the Blazor clients do not need new error-handling logic.

### 10.5 Do Not Add API Logic to the Application Layer

Controllers are thin. Business logic stays in the Application services. A controller that does more than:

1. Read from `HttpContext` / route params
2. Call one service method
3. Return the result

...is doing too much. Move any extra logic down to the service layer.

### 10.6 DTO Location Decision — Phase 8 Dependency

When Phase 8 removes the Blazor project's reference to `BeHealthy.Application`, the Blazor app will no longer be able to use DTOs from `BeHealthy.Application\Dtos\`. Two options to resolve this:

**Option A (Recommended):** Move all DTOs from `BeHealthy.Application\Dtos\` into `BeHealthy.Shared\`. Both the API and the Blazor client reference Shared. Simple, no new project needed.

**Option B:** Create a new `BeHealthy.Contracts` project that holds only DTOs and query parameters. Both API and Blazor reference it. Application references it too (replaces its own Dtos folder). Cleaner long-term but more initial work.

**Decision to be made before Phase 8 begins.** Do not act on this earlier.

---

*End of document. Update the checklist items as work is completed.*
