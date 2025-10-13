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

### 3. Set docker-compose as the Startup project and run the app

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
  -e POSTGRES_PASSWORD=7530 \
  -p 5432:5432 \
  postgres:latest
```
> ⚠️ Note: If we follow this approach then we should change the Host of the connection string to localhost
<img width="1701" height="205" alt="image" src="https://github.com/user-attachments/assets/67151a1a-9ca2-4d12-ae01-5a2a76e0b5d0" />

**b) Set the blazor project as a Startup and run it**
<img width="1300" height="350" alt="image" src="https://github.com/user-attachments/assets/7908fc20-12ff-46de-9578-f21ac0200fb7" />


### 5. Admin User

An admin user is automatically seeded for testing:

Email: admin@gmail.com
Password: 123456aA@

You can log in with this user, or create a new admin account via the Register form.

> ⚠️ Note: Although other users exist, their pages are not fully implemented. It is recommended to browse the application as an admin to test products, navigation, and management features.
