# SQL Scripts For DeliveryApp

Executer les scripts dans Azure Data Studio dans cet ordre:

1. `03_auth_user_accounts.sql`
2. `01_seed_reference_data.sql`
3. `02_simulation_scenario.sql` (optionnel, jeu d'exemple)

Notes:

- `03_auth_user_accounts.sql` cree `UserAccounts` pour le mode JWT/roles.
- `01_seed_reference_data.sql` insere des donnees de reference minimales.
- `02_simulation_scenario.sql` cree un jeu d'exemple complet (commande + paiement + avis).
- Les scripts visent le schema EF Core courant:
  - `Customers`
  - `Addresses`
  - `Stores`
  - `Products`
  - `Couriers`
  - `Orders`
  - `OrderItems`
  - `Payments`
  - `Reviews`
  - `UserAccounts`
