# DeliveryApp - Complete Documentation

## 1. Project Vision

DeliveryApp is an Uber Eats-like delivery application with:

- an ASP.NET Core backend (REST API + JWT + roles)
- a Blazor Server frontend (role-based application)
- a SQL Server database

The repository combines two dimensions:

- `Legacy runtime`: Domain/Application/Infrastructure/Service layers currently used in runtime code
- `Target architecture`: modular monolith (`Modules/*` + `BuildingBlocks/*`) for progressive migration

## 2. Covered Features

### Customer Side

- JWT signup and login
- browse stores and products
- add items to cart
- create orders
- track personal orders
- payment
- reviews

### Courier Side

- JWT signup and login
- orders board access
- order assignment
- delivery status progression

### Admin Side

- bootstrap admin signup + JWT login
- store management (create, update, delete)
- store address management
- account moderation (block/unblock)
- order supervision
- full administration access

## 3. Technical Architecture

## 3.1 Main Projects (active runtime)

- `src/DeliveryApp.Api`: HTTP endpoints, JWT auth, policies, swagger
- `src/DeliveryApp.Application`: DTOs, interfaces, validations, `ServiceResult`
- `src/DeliveryApp.Domain`: domain entities and enums
- `src/DeliveryApp.Infrastructure`: EF Core, `AppDbContext`, SQL repositories
- `src/DeliveryApp.Service`: business services
- `src/DeliveryApp.Frontend`: role-based Blazor UI

## 3.2 Target Modular Monolith Projects

- `src/BuildingBlocks/*`: shared abstractions (`Result`, `Messaging`, `Entity`, `IUnitOfWork`)
- `src/Modules/Catalog/*`
- `src/Modules/Ordering/*`
- `src/Modules/Dispatch/*`
- `src/Modules/Payments/*`
- `src/Modules/Identity/*`

Important:
- `/api/modules/*` routes are wired through adapters and still reuse legacy services
- `Modules.Identity` currently contains a stub and is not the runtime auth engine yet

## 4. Tech Stack

- .NET 10 (`net10.0`)
- ASP.NET Core Minimal APIs
- Entity Framework Core 10 + SQL Server
- JWT Bearer Authentication
- Swagger / OpenAPI
- Interactive Blazor Server

## 5. Repository Structure

```text
DeliveryApp.slnx
docs/
  README.md
  runbook-backend.md
  api/
  architecture/
  sql/
src/
  DeliveryApp.Api/
  DeliveryApp.Application/
  DeliveryApp.Domain/
  DeliveryApp.Infrastructure/
  DeliveryApp.Service/
  DeliveryApp.Frontend/
  BuildingBlocks/
  Modules/
```

## 6. Data Model (main)

Business tables:

- `Customers`
- `Addresses`
- `Stores`
- `Products`
- `Couriers`
- `Orders`
- `OrderItems`
- `Payments`
- `Reviews`
- `UserAccounts` (JWT auth)

Notable constraints:

- `UserAccounts.Email` unique
- `UserAccounts.CustomerId` filtered unique index (non-null)
- `UserAccounts.CourierId` filtered unique index (non-null)
- `Orders -> Addresses` relationship uses `Restrict`
- `OrderItems -> Orders` relationship uses `Cascade`

## 7. Authentication and Authorization

## 7.1 JWT

API configuration:

- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:Secret` (minimum 32 characters)
- `Jwt:AccessTokenMinutes`

Used claims:

- `sub`, `name`, `email`, `role`
- `customerId` (for Customer role)
- `courierId` (for Courier role)

## 7.2 Roles

- `Customer`
- `Courier`
- `Admin`

Defined policies:

- `CustomerOnly`
- `CourierOnly`
- `AdminOnly`
- `CourierOrAdmin`

## 7.3 Admin bootstrap

When an admin already exists, admin registration requires:

- `Auth:AdminBootstrapKey`

Current default dev value:

- `deliveryapp-bootstrap-admin-2026`

## 8. API Endpoints

Base URL with `http` launch profile:

- `http://localhost:5000`

Health:

- `GET /health` (public)

Auth:

- `POST /api/auth/login` (public)
- `POST /api/auth/register/customer` (public)
- `POST /api/auth/register/courier` (public)
- `POST /api/auth/register/admin` (public, bootstrap key depending on context)
- `GET /api/auth/me` (auth required)

Account:

- `GET /api/account/context` (auth required)
- `GET /api/account/addresses` (`CustomerOnly`)
- `POST /api/account/addresses` (`CustomerOnly`)

Reference management (admin):

- `GET /api/customers` (`AdminOnly`)
- `POST /api/addresses` (`AdminOnly`)
- `GET /api/addresses` (`AdminOnly`)
- `POST /api/stores` (`AdminOnly`)
- `PUT /api/stores/{storeId}` (`AdminOnly`)
- `DELETE /api/stores/{storeId}` (`AdminOnly`)
- `POST /api/products` (`AdminOnly`)
- `GET /api/auth/admin/accounts` (`AdminOnly`)
- `PATCH /api/auth/admin/accounts/{accountId}/status` (`AdminOnly`)

Read operations:

- `GET /api/stores` (public)
- `GET /api/stores/{storeId}/products` (public)
- `GET /api/couriers` (`CourierOrAdmin`)
- `GET /api/couriers/available` (`CourierOrAdmin`)

Orders:

- `GET /api/orders/mine` (auth required, role-based filtering)
- `GET /api/orders` (`CourierOrAdmin`)
- `GET /api/orders/{id}` (auth required, role-based access control)
- `GET /api/orders/status/{status}` (`CourierOrAdmin`)
- `POST /api/orders` (`CustomerOnly`)
- `PUT /api/orders/{id}` (`CourierOnly`)
- `PATCH /api/orders/{orderId}/assign-courier` (`CourierOnly`)
- `DELETE /api/orders/{id}` (`AdminOnly`)

Payments:

- `POST /api/orders/{orderId}/payments` (`CustomerOnly`, with order access checks)
- `GET /api/orders/{orderId}/payments` (auth required, with order access checks)

Reviews:

- `POST /api/orders/{orderId}/reviews` (`CustomerOnly`, with order access checks)
- `GET /api/orders/{orderId}/reviews` (auth required, with order access checks)

Module routes (`/api/modules/*`):

- `GET /api/modules/catalog/stores` (`AdminOnly`)
- `GET /api/modules/catalog/stores/{storeId}/products` (`AdminOnly`)
- `POST /api/modules/ordering/orders` (`CustomerOnly`)
- `GET /api/modules/ordering/orders/{orderId}` (auth required)
- `PATCH /api/modules/dispatch/orders/{orderId}/couriers/{courierId}` (`CourierOnly`)
- `GET /api/modules/dispatch/couriers/available` (`CourierOnly`)
- `POST /api/modules/payments/pay` (`CustomerOnly`)
- `GET /api/modules/payments/orders/{orderId}` (auth required)

## 9. Main Business Rules

Orders:

- at least 1 item
- strictly positive quantities
- all products must belong to the selected store
- status progression step by step (`N -> N+1`) or cancellation
- closed orders (`Delivered` or `Cancelled`) cannot be modified

Dispatch:

- an unavailable courier cannot be assigned to a new order
- assigning a courier to a `Pending` order moves it to `Accepted`

Payment:

- amount must equal order total
- only one `Completed` payment per order

Review:

- allowed only when order is `Delivered`
- only one review per order

## 10. Blazor Frontend (role-based usage)

Base URL with `http` launch profile:

- `http://localhost:5105`

Session:

- JWT token stored in `AuthSessionState` (Blazor Server scope)
- `DeliveryApiClient` automatically adds `Authorization: Bearer ...`

Page access by role:

- Public: `/account`, `/forbidden`, `/not-found`
- Customer: `/stores`, `/stores/{id}`, `/cart`, `/my-orders`
- Courier: `/orders/board`, `/orders`, `/orders/{id}`
- Admin: `/admin`, `/dashboard`, `/orders/{id}` (read-only)

Main workflow:

1. login/register through `/account`
2. customer -> select store -> cart -> checkout
3. create order -> tracking
4. courier -> assign + advance order status
5. customer -> payment + review

## 11. Setup and Run

Prerequisites:

- .NET SDK 10
- reachable SQL Server (example: `localhost,1433`)
- Azure Data Studio or SSMS for SQL scripts

From repository root:

```bash
dotnet restore
```

## 11.1 Database

Option A (recommended): EF Core migrations

```bash
dotnet ef database update --project src/DeliveryApp.Infrastructure/DeliveryApp.Infrastructure.csproj --startup-project src/DeliveryApp.Api/DeliveryApp.Api.csproj
```

Option B: existing database

- ensure the schema contains all required tables
- run `docs/sql/03_auth_user_accounts.sql` if `UserAccounts` is missing

Reference seed (recommended order):

1. `docs/sql/01_seed_reference_data.sql`
2. `docs/sql/02_simulation_scenario.sql` (optional, sample dataset)

## 11.2 Run API and Frontend

Terminal 1 (API):

```bash
dotnet run --project src/DeliveryApp.Api --launch-profile http
```

Terminal 2 (Frontend):

```bash
dotnet run --project src/DeliveryApp.Frontend --launch-profile http
```

URLs:

- API: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`
- Frontend: `http://localhost:5105`

## 12. Quick Test Procedure

## 12.1 API Test (Swagger/Postman)

1. create or login an account via `/api/auth/*`
2. retrieve `accessToken`
3. send `Bearer <token>` in secured requests
4. test by role:
- Customer: order + payment + review
- Courier: board + assign + status update
- Admin: full administration + supervision

## 12.2 Frontend Test

1. open `http://localhost:5105/account`
2. login or register
3. follow role-based navigation
4. verify consistency between UI and API data

## 13. Auth SQL Utility

Script:

- `docs/sql/03_auth_user_accounts.sql`

Purpose:

- create `UserAccounts` table + constraints + indexes
- quick verification with `SELECT TOP 5 * FROM dbo.UserAccounts`

## 14. Error Handling

Response format:

- `Results.Problem(...)` with `title`, `detail`, `statusCode`
- `errorCode` extension for business errors

Common HTTP codes:

- `400`: validation
- `401`: invalid credentials / invalid token
- `403`: forbidden / admin bootstrap denied
- `404`: resource not found
- `409`: business conflict
- `500`: unexpected server error

## 15. Current Caveats

- module projects exist, but core runtime logic still mostly lives in legacy services
- `Modules.Identity` is not yet the runtime auth engine
- no automated test suite in repository yet (integration/e2e to add)
- dev secrets are visible in `appsettings*.json`: externalize before production

## 16. Additional Documentation

- `docs/runbook-backend.md`
- `docs/sql/README.md`
- `docs/api/frontend-workflow.md`
- `docs/api/workflow-scenario.md`
- `docs/api/usage-checklist.md`
- `docs/architecture/README.md`
- `docs/architecture/current-state.md`
- `docs/architecture/modular-monolith-migration.md`
- PlantUML diagrams in `docs/architecture/*.puml`

## 17. "Backend Operational" Definition

Backend is considered operational when:

- migrations/schema are OK
- JWT auth endpoints are working
- role policies are correctly enforced
- order creation and tracking are working
- payment + review flows follow business rules
- Blazor frontend is connected and usable per role

This status is reached in local development mode with SQL seed applied and two running processes (`API` + `Frontend`).
