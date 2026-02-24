# Backend Runbook (API + DB + JWT)

## 1. Prerequis

- .NET SDK 10
- SQL Server disponible
- base `DeliveryAppDb` accessible via la connection string de `src/DeliveryApp.Api/appsettings.json`

## 2. Initialiser la base

Depuis la racine du repo:

```bash
dotnet ef database update --project src/DeliveryApp.Infrastructure/DeliveryApp.Infrastructure.csproj --startup-project src/DeliveryApp.Api/DeliveryApp.Api.csproj
```

Dans Azure Data Studio (ordre recommande):

1. `docs/sql/03_auth_user_accounts.sql`
2. `docs/sql/01_seed_reference_data.sql`
3. `docs/sql/02_simulation_scenario.sql` (optionnel, jeu d'exemple)

## 3. Lancer le backend

```bash
dotnet run --project src/DeliveryApp.Api --launch-profile http
```

URLs:

- API: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`
- Health: `http://localhost:5000/health`

## 4. Verification rapide JWT

1. `POST /api/auth/register/customer` ou `POST /api/auth/login`
2. recuperer `accessToken`
3. tester `GET /api/auth/me` avec `Authorization: Bearer <token>`

## 5. Endpoints essentiels

Public:

- `GET /health`
- `GET /api/stores`
- `GET /api/stores/{storeId}/products`
- `POST /api/auth/login`
- `POST /api/auth/register/customer`
- `POST /api/auth/register/courier`
- `POST /api/auth/register/admin`

Customer:

- `POST /api/orders`
- `POST /api/orders/{orderId}/payments`
- `POST /api/orders/{orderId}/reviews`
- `GET /api/account/addresses`
- `POST /api/account/addresses`

Courier:

- `PATCH /api/orders/{orderId}/assign-courier`
- `PUT /api/orders/{orderId}`
- `GET /api/orders`

Admin:

- `POST /api/addresses`
- `POST /api/stores`
- `PUT /api/stores/{storeId}`
- `DELETE /api/stores/{storeId}`
- `POST /api/products`
- `GET /api/auth/admin/accounts`
- `PATCH /api/auth/admin/accounts/{accountId}/status`

## 6. Lancer le frontend en parallele (optionnel)

```bash
dotnet run --project src/DeliveryApp.Frontend --launch-profile http
```

URL frontend:

- `http://localhost:5105`
