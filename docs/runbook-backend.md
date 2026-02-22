# Backend Runbook (DB + Simulation)

## 1. Apply database migrations

Run from repository root:

```bash
dotnet ef database update --project src/DeliveryApp.Infrastructure/DeliveryApp.Infrastructure.csproj --startup-project src/DeliveryApp.Api/DeliveryApp.Api.csproj
```

## 2. Seed simulation reference data

Run in Azure Data Studio:

1. `docs/sql/01_seed_reference_data.sql`
2. `docs/sql/02_simulation_scenario.sql` (optional fast scenario)

## 3. Start API

```bash
dotnet run --project src/DeliveryApp.Api/DeliveryApp.Api.csproj
```

Swagger in development:
- `https://localhost:5001/swagger` (or port shown in console)

## 4. Core endpoints to validate

- `GET /health`
- `GET /api/customers`
- `GET /api/addresses`
- `GET /api/stores`
- `GET /api/stores/{storeId}/products`
- `GET /api/couriers`
- `GET /api/couriers/available`
- `GET /api/orders`
- `GET /api/orders/{id}`
- `POST /api/orders`
- `PATCH /api/orders/{id}/assign-courier`
- `POST /api/orders/{id}/payments`
- `GET /api/orders/{id}/payments`
- `POST /api/orders/{id}/reviews`
- `GET /api/orders/{id}/reviews`

## 5. Module endpoints (migration active)

- `GET /api/modules/catalog/stores`
- `GET /api/modules/catalog/stores/{storeId}/products`
- `POST /api/modules/ordering/orders`
- `GET /api/modules/ordering/orders/{orderId}`
- `PATCH /api/modules/dispatch/orders/{orderId}/couriers/{courierId}`
- `GET /api/modules/dispatch/couriers/available`
- `POST /api/modules/payments/pay`
- `GET /api/modules/payments/orders/{orderId}`
