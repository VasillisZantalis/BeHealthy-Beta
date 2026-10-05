# BeHealthy (Beta)

BeHealthy is a healthcare management application for hospitals and clinics: doctors, nurses, patients, appointments, rooms, departments and patient medical history in one place.

It is built with **Clean Architecture**: a **Blazor Server** front end that talks to an **ASP.NET Core Web API**, which stores its data in **SQLite**.

> ⚠️ Note: Only the admin user and functionality have been fully tested. Other roles (doctor, nurse, patient) exist, but some of their pages are not fully implemented. It is recommended to explore the application as an admin.

---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- Optional: Visual Studio 2022 (17.12+) or JetBrains Rider

No database server is needed: SQLite stores everything in a single file (`behealthy.db`) that the API creates on startup.

---

## Key Features

- **Dashboard**: totals for doctors, nurses and patients, upcoming appointments, recently added patients, a calendar, and (for admins) user-distribution and appointment-reason charts.
- **Doctor, Nurse and Patient management**: create, edit, delete and search, with department and specialty assignment and profile images.
- **Patient records**: allergies, prescriptions, medical records and visits per patient.
- **Appointments**: book appointments between doctor and patient (optionally with a nurse and room), with conflict detection so nobody is double-booked.
- **Rooms, Departments and Specialties** management.
- **Settings** (admin): toggle business rules such as "appointment requires a room" or "doctor must have a specialty". They are enforced in both the UI and the API.
- **Mass Import**: add many rows at once in a grid on the Doctors, Nurses, Patients and Appointments pages.
- **Tools → Mass Import** (admin, top navigation bar): fill the database with realistic sample doctors, patients, nurses and appointments (generated with Bogus).
- **Authentication & roles**: ASP.NET Core Identity with Admin, Doctor, Nurse and Patient roles.

## 🚀 Upcoming Features (Planned)

- Complete pages for all user types to view their relevant data
- Connected user profile page for managing personal information
- Internal notification system to alert users about relevant actions
- Patient-specific features including diagnoses, treatments, and lab results

---

## Technologies Used

| Area | Technology |
|---|---|
| Platform | .NET 9 |
| Front end | Blazor Server (interactive server rendering), Bootstrap, Font Awesome |
| Charts & calendar | ChartJs.Blazor, FullCalendar |
| Back end | ASP.NET Core Web API, OpenAPI with Scalar UI |
| Data | Entity Framework Core with SQLite |
| Auth | ASP.NET Core Identity, JWT bearer tokens (API) and cookie authentication (front end) |
| Validation | FluentValidation, shared by the front end and the API |
| Sample data | Bogus |
| Tests | xUnit, bUnit, Moq, Shouldly, AutoFixture |

## Architecture & Patterns

- **Clean Architecture**: Domain, Application, Infrastructure and API layers, with the Blazor front end as a separate client of the API.
- **Front end ↔ API**: the front end signs the user in with a cookie that carries the JWT the API issued, and sends that JWT on every API call.
- **Repository Pattern**: data access is abstracted behind repository interfaces in the Application layer.
- **Transactions**: multi-step writes (for example, creating an Identity user and the doctor that belongs to it) run in one explicit transaction.
- **Validation**: FluentValidation rules are shared by the front end and the API and enforced on every API request.
- **Component-Based UI**: reusable Blazor components (tables, modals, wizards, form controls).
- **State Management**: scoped services for UI state (modals, breadcrumbs, toasts, loaders).

## Solution Structure

Projects live under `src/` (application code) and `tests/`. In Visual Studio they are grouped into solution folders:

| Solution folder | Project | Purpose |
|---|---|---|
| `src/Backend` | `BeHealthy.Domain` | Entities and domain enums |
| `src/Backend` | `BeHealthy.Application` | Services, repository interfaces, server-only validators, sample-data seeding |
| `src/Backend` | `BeHealthy.Infrastructure` | EF Core (SQLite), Identity, repositories, migrations |
| `src/Backend` | `BeHealthy.API` | ASP.NET Core Web API: controllers, validation filter |
| `src/Frontend` | `BeHealthy.Front` | Blazor Server front end; talks to the API |
| `src/Shared` | `BeHealthy.Shared` | Request/response DTOs, enums, UI strings |
| `src/Shared` | `BeHealthy.Validation` | Request validators used by both the Front and the API |
| `tests` | `BeHealthy.Tests` | xUnit and bUnit tests |

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/VasillisZantalis/BeHealthy-Beta.git
cd BeHealthy-Beta
```

### 2. Run the API and the front end

Both projects must run at the same time: the front end has no data of its own and calls the API for everything.

| Project | URL |
|---|---|
| `BeHealthy.API` | https://localhost:7187 (API docs at https://localhost:7187/scalar) |
| `BeHealthy.Front` | https://localhost:7130 |

**Visual Studio:** right-click the solution → **Configure Startup Projects…** → **Multiple startup projects**, set `BeHealthy.API` and `BeHealthy.Front` to **Start**, then press F5.

**Command line:** use two terminals from the repository root:

```bash
dotnet run --project src/BeHealthy.API --launch-profile https
dotnet run --project src/BeHealthy.Front --launch-profile https
```

Then open https://localhost:7130.

> If the browser warns about the certificate, trust the .NET development certificate once with `dotnet dev-certs https --trust`.

### 3. Log in as the admin

An admin user is created automatically when the API starts:

- **Email:** `admin@gmail.com`
- **Password:** `123456aA@`

There is no public registration form. The admin creates doctors, nurses and patients, and each of them gets their own login.

### 4. Add sample data

The database starts empty. As the admin, open **Tools → Mass Import** in the top navigation bar, choose how many doctors, patients, nurses and appointments to create, and press **Save**.

Seeded users get realistic names and `@behealthy.com` emails, and you can log in as any of them with these passwords:

| Role | Password |
|---|---|
| Doctor | `Doctor123!` |
| Patient | `Patient123!` |
| Nurse | `Nurse123!` |

Create a few departments, specialties and rooms first if you want the seeded people and appointments to be linked to them.

> ⚠️ **The database is reset on every start in Development.** The API deletes and recreates `behealthy.db` each time it starts, so all data (including seeded data) is lost on restart.

---

## Running the Tests

```bash
dotnet test tests/BeHealthy.Tests
```