# SQL Scripts For Simulation

Run scripts in Azure Data Studio in this order:

1. `01_seed_reference_data.sql`
2. `02_simulation_scenario.sql`

Notes:
- Scripts are idempotent where possible.
- They target the current EF schema (`Customers`, `Addresses`, `Stores`, `Products`, `Couriers`, `Orders`, `OrderItems`, `Payments`, `Reviews`).
