# BeHealthy Blazor Project (Beta)

This repository contains the **BeHealthy Blazor project** using **Clean Architecture** and **PostgreSQL** as the database.  
It has been configured to run with **Docker Compose** and uses **user-secrets** for connection strings.

> ⚠️ Note: Only the admin user and functionality have been fully tested. Other users exist but some pages are not fully implemented. It is recommended to explore the application as an admin.

---

## Prerequisites

- [Docker](https://www.docker.com/get-started)  
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/VasillisZantalis/BeHealthy-Beta.git
cd BeHealthy-Beta
```

### 2. Set up the Database Connection String with User-Secrets
The project uses user-secrets in the Infrastructure project to store the database connection string securely.

Navigate to the Infrastructure project folder:

```bash
cd src/BeHealthy.Infrastructure
```

Initialize user-secrets (if not already initialized):

```bash
dotnet user-secrets init
```

Set the connection string:
```bash
dotnet user-secrets set "ConnectionStrings:Default" "Host=behealthydb;Port=5432;Database=behealthy;Username=admin;Password=7530"
```

Explanation:
Host should match the service name in docker-compose.yml if using Docker (behealthydb).
Port is the PostgreSQL port (default 5432).
Database is the database name (behealthy).
Username and Password are the database credentials.

### 3. Start Docker Compose

From the root of the project:
```bash
docker-compose up -d
```

### 4. Start the app

<img width="1134" height="346" alt="image" src="https://github.com/user-attachments/assets/5b22d312-3b8b-44e3-9eaf-209e25155c8d" />


### 5. Admin User

An admin user is automatically seeded for testing:

Email: admin@gmail.com
Password: 123456aA@

You can log in with this user, or create a new admin account via the Register form.

> ⚠️ Note: Although other users exist, their pages are not fully implemented. It is recommended to browse the application as an admin to test products, navigation, and management features.
