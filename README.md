# DeliveryApp

DeliveryApp is a food delivery application (Uber Eats style) built with a .NET backend, a Blazor Server frontend, and SQL Server persistence.

## Project Description

The project provides a role-based experience for:

- `Customer`: browse stores, place orders, pay, and leave reviews
- `Courier`: accept and progress deliveries
- `Admin`: manage stores, moderation, and operations

The repository currently contains:

- an active layered runtime (`Api`, `Application`, `Service`, `Infrastructure`, `Domain`)
- a target modular monolith structure under `src/Modules/*` and `src/BuildingBlocks/*`

## Architecture Overview

Current runtime flow:

```text
Users (Customer/Courier/Admin)
  -> Blazor Frontend (src/DeliveryApp.Frontend)
  -> ASP.NET Core API (src/DeliveryApp.Api)
  -> Business Services (src/DeliveryApp.Service)
  -> Repositories + EF Core (src/DeliveryApp.Infrastructure)
  -> SQL Server (DeliveryAppDb)
```

Main projects:

- `src/DeliveryApp.Api`: minimal APIs, JWT auth, authorization policies, Swagger
- `src/DeliveryApp.Application`: DTOs, interfaces, application contracts
- `src/DeliveryApp.Service`: business logic
- `src/DeliveryApp.Infrastructure`: EF Core `AppDbContext`, repositories, migrations
- `src/DeliveryApp.Domain`: entities and enums
- `src/DeliveryApp.Frontend`: Blazor Server client

Modular note:

- `src/Modules/*` exists and is being introduced progressively
- the main runtime behavior is still mostly handled by the layered stack above

## SQL & Cosmos Configuration

### SQL Server (active)

API configuration files:

- `src/DeliveryApp.Api/appsettings.json`
- `src/DeliveryApp.Api/appsettings.Development.json`

Connection string key:

- `ConnectionStrings:DefaultConnection`

Example format:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=DeliveryAppDb;User Id=sa;Password=***;TrustServerCertificate=True;"
  }
}
```

### Cosmos DB (not active yet)

Cosmos DB is currently not wired into runtime:

- no Cosmos package references in active projects
- no Cosmos settings in `appsettings*.json`
- no Cosmos repositories/services registered in the API startup

It can later be introduced for read models and timeline/event-style data, while keeping SQL Server as the transactional source.

## Migrations

Prerequisites:

- .NET 10 SDK
- SQL Server instance running
- EF CLI tool (`dotnet-ef`)

Install EF CLI if needed:

```bash
dotnet tool install --global dotnet-ef
```

Apply existing migrations:

```bash
dotnet ef database update --project src/DeliveryApp.Infrastructure/DeliveryApp.Infrastructure.csproj --startup-project src/DeliveryApp.Api/DeliveryApp.Api.csproj
```

Create a new migration:

```bash
dotnet ef migrations add <MigrationName> --project src/DeliveryApp.Infrastructure/DeliveryApp.Infrastructure.csproj --startup-project src/DeliveryApp.Api/DeliveryApp.Api.csproj --output-dir Data/Migrations
```

Optional SQL seed scripts (recommended order):

1. `docs/sql/03_auth_user_accounts.sql`
2. `docs/sql/01_seed_reference_data.sql`
3. `docs/sql/02_simulation_scenario.sql` (optional demo data)

## How to Run API

From the repository root:

```bash
dotnet restore
dotnet run --project src/DeliveryApp.Api --launch-profile http
```

Default local endpoints:

- API: `http://localhost:5000`
- Health: `http://localhost:5000/health`
- Swagger: `http://localhost:5000/swagger`

## How to Run Client

Check frontend API target first:

- file: `src/DeliveryApp.Frontend/appsettings.json`
- key: `ApiBaseUrl` (default: `http://localhost:5000`)

Run the client:

```bash
dotnet run --project src/DeliveryApp.Frontend --launch-profile http
```

Default local URL:

- Frontend: `http://localhost:5105`

## Project Demonstration (YouTube)

- [Project Demonstration on YouTube](https://youtu.be/WZrxehRaHxA)

## Additional Documentation

- `docs/README.md`
- `docs/architecture/README.md`
- `docs/architecture/architecture-document.fr.md`
- `docs/api/frontend-workflow.md`
- `docs/sql/README.md`
