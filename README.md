# BeHealthy Blazor Project (Beta)

This repository contains the **BeHealthy Blazor project** using **Clean Architecture** and **PostgreSQL** as the database.  
It has been configured to run with **Docker Compose** and uses **user-secrets** for connection strings.

> ⚠️ Note: Only the admin user and functionality have been fully tested. Other users exist but some pages are not fully implemented. It is recommended to explore the application as an admin.

---

## Prerequisites

- [Docker](https://www.docker.com/get-started)  
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

---

## Overview
BeHealthy is a modern healthcare management solution designed to streamline patient, doctor, and medical record workflows. Built as a Blazor Server application, it provides an interactive, responsive user experience.

## Technologies Used
- **.NET 9**: Latest .NET platform for performance and security.
- **Blazor Server**: Rich, interactive UI with real-time updates.
- **Entity Framework Core (Npgsql)**: PostgreSQL database integration.
- **ASP.NET Core Identity**: Secure authentication and user management.
- **Serilog**: Structured logging to console and file.
- **ChartJs.Blazor**: Data visualization and charting.
- **FluentValidation**: Robust form validation.

## Key Features
- **User Authentication**: Secure login, registration, and role management.
- **Patient Management**: Create, view, and edit patient profiles.
- **Medical Records**: Add, update, and list medical records per patient.
- **Doctor Management**: Assign and manage doctors.
- **Appointments Managmenet**: Create appointments between doctor and patient
- **Localization**: Multi-language support (English, Greek).

## 🚀 Upcoming Features (Planned)
- Complete pages for all user types to view their relevant data  
- Connected user profile page for managing personal information  
- Internal notification system to alert users about relevant actions  
- Patient-specific features including diagnoses, treatments, and lab result
  
## Architecture & Patterns
- **Layered Architecture**: Separation of concerns via Application, Infrastructure, and Domain layers.
- **Dependency Injection**: Decoupled service registration and resolution.
- **Repository Pattern**: Abstracted data access for maintainability.
- **Component-Based UI**: Reusable Blazor components for modularity.
- **State Management**: Scoped services for UI state (modals, navigation, loaders).

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/VasillisZantalis/BeHealthy-Beta.git
cd BeHealthy-Beta
```

### 2. Set docker-compose as the Startup project and run the app

<img width="1134" height="346" alt="image" src="https://github.com/user-attachments/assets/5b22d312-3b8b-44e3-9eaf-209e25155c8d" />


## Alternative: Running without Docker Compose

If you prefer not to use Docker Compose or encountered an error, you can run the PostgreSQL database manually and then start the Blazor application directly from Visual Studio.

### 1. Run PostgreSQL manually

You can Use Docker manually:

**a) Run the following command to create the container**
```bash
docker run -d \
  --name behealthydb \
  -e POSTGRES_DB=behealthy \
  -e POSTGRES_USER=admin \
  -e POSTGRES_PASSWORD=123456asd!@# \
  -p 5432:5432 \
  postgres:latest
```
> ⚠️ **Note**: If we follow this approach then we should change the Host of the connection string to **localhost**

**b) Set the blazor project as a Startup and run it**
<img width="1300" height="350" alt="image" src="https://github.com/user-attachments/assets/7908fc20-12ff-46de-9578-f21ac0200fb7" />


### 5. Admin User

An admin user is automatically seeded for testing:

Email: admin@gmail.com
Password: 123456aA@

You can log in with this user, or create a new admin account via the Register form.

> ⚠️ Note: Although other users exist, their pages are not fully implemented. It is recommended to browse the application as an admin to test products, navigation, and management features.
