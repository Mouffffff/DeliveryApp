# Current State

The repository now contains:

- Legacy vertical slice still used by the API:
  - `DeliveryApp.Domain`
  - `DeliveryApp.Application`
  - `DeliveryApp.Infrastructure`
  - `DeliveryApp.Service`
- Target modular monolith structure:
  - `BuildingBlocks/*`
  - `Modules/Catalog/*`
  - `Modules/Ordering/*`
  - `Modules/Dispatch/*`
  - `Modules/Payments/*`
  - `Modules/Identity/*`

Active state:

- Legacy endpoints remain available and fully operational.
- Module services are now active through adapters (Catalog, Ordering, Dispatch, Payments) and registered in DI.
- New module-oriented API routes are available under `/api/modules/*` for progressive migration and validation.
- Fat service/repository were removed from runtime wiring:
  - `IDeliveryService` removed
  - `IDeliveryRepository` removed
  - API now depends on specialized services and repositories.

Next step is to replace legacy endpoints by module endpoints and then remove legacy projects.
