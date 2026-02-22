# Migration Plan To Modular Monolith

Current projects remain operational. New modules were scaffolded to migrate safely in phases.

## Phase 1 (done)
- Create `BuildingBlocks` projects.
- Create `Modules/*` projects for `Catalog`, `Ordering`, `Dispatch`, `Payments`, `Identity`.
- Add project references and compile each module independently.

## Phase 2 (next)
- Move existing order endpoints and service logic into `Modules/Ordering`.
- Keep old API routes, but delegate to module application services.

## Phase 3
- Move store/product queries into `Modules/Catalog`.
- Move courier assignment into `Modules/Dispatch`.
- Move payment logic into `Modules/Payments`.

## Phase 4
- Add authentication/authorization in `Modules/Identity`.
- Add integration tests for each module.
