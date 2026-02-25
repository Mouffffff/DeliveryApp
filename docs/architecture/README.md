# DeliveryApp - Architecture README

Ce README sert de point d'entree pour comprendre rapidement le projet DeliveryApp et le lancer en local.

## 1. Project description

DeliveryApp est une application de livraison type Uber Eats avec:

- un backend `ASP.NET Core` (`Minimal APIs`, JWT, roles)
- un frontend `Blazor Server`
- une base relationnelle `SQL Server` via `Entity Framework Core`

Le repository combine deux dimensions:

- runtime actuel en couches (`DeliveryApp.Api`, `DeliveryApp.Service`, `DeliveryApp.Infrastructure`, etc.)
- cible modulaire (`src/Modules/*` + `src/BuildingBlocks/*`) en migration progressive

## 2. Architecture overview

### Vue d'ensemble runtime (etat actuel)

```text
Users (Customer/Courier/Admin)
    -> Frontend Blazor Server (src/DeliveryApp.Frontend)
    -> Backend API .NET (src/DeliveryApp.Api)
    -> Services metier (src/DeliveryApp.Service)
    -> Repositories + EF Core (src/DeliveryApp.Infrastructure)
    -> SQL Server (DeliveryAppDb)
```

### Couches principales

- `src/DeliveryApp.Api`: endpoints, auth JWT, policies, Swagger
- `src/DeliveryApp.Application`: DTOs, interfaces, validation, contrats applicatifs
- `src/DeliveryApp.Service`: logique metier (orders, payments, review, auth, etc.)
- `src/DeliveryApp.Infrastructure`: `AppDbContext`, repositories SQL, migrations EF
- `src/DeliveryApp.Domain`: entites et enums metier
- `src/DeliveryApp.Frontend`: interface utilisateur Blazor role-based

### Etat de migration modulaire

Le dossier `src/Modules/*` (Catalog, Ordering, Dispatch, Payments, Identity) est present.
Aujourd'hui, la logique principale runtime reste majoritairement sur la stack legacy en couches.

## 3. SQL & Cosmos configuration

### SQL Server (actif)

Configuration actuelle dans:

- `src/DeliveryApp.Api/appsettings.json`
- `src/DeliveryApp.Api/appsettings.Development.json`

Cle utilisee par l'API:

- `ConnectionStrings:DefaultConnection`

Exemple local:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=DeliveryAppDb;User Id=sa;Password=***;TrustServerCertificate=True;"
  }
}
```

Important:

- la chaine de connexion est lue au demarrage de `DeliveryApp.Api`
- le `DbContext` utilise SQL Server avec assembly de migrations `DeliveryApp.Infrastructure`

### Cosmos DB (non actif pour le moment)

Cosmos DB n'est pas implemente dans le runtime actuel:

- pas de package Cosmos dans les `.csproj` actifs
- pas de section de configuration Cosmos dans `appsettings*.json`
- pas de repository/service Cosmos branche dans l'API

Utilisation cible possible (documentee dans `architecture-document.fr.md`):

- read models denormalises
- timeline d'evenements de commande
- vues de lecture rapides a grande echelle

## 4. Migrations

### Pre-requis

- .NET SDK 10 installe
- SQL Server accessible
- outil `dotnet-ef` disponible

Si besoin:

```bash
dotnet tool install --global dotnet-ef
```

### Appliquer les migrations existantes

Depuis la racine du repo:

```bash
dotnet ef database update --project src/DeliveryApp.Infrastructure/DeliveryApp.Infrastructure.csproj --startup-project src/DeliveryApp.Api/DeliveryApp.Api.csproj
```

### Creer une nouvelle migration

```bash
dotnet ef migrations add <MigrationName> --project src/DeliveryApp.Infrastructure/DeliveryApp.Infrastructure.csproj --startup-project src/DeliveryApp.Api/DeliveryApp.Api.csproj --output-dir Data/Migrations
```

### Seed SQL complementaire

Les scripts du dossier `docs/sql` peuvent etre executes apres migration (selon le besoin):

1. `docs/sql/03_auth_user_accounts.sql`
2. `docs/sql/01_seed_reference_data.sql`
3. `docs/sql/02_simulation_scenario.sql` (optionnel)

## 5. How to run API

Depuis la racine:

```bash
dotnet restore
dotnet run --project src/DeliveryApp.Api --launch-profile http
```

Endpoints utiles:

- API: `http://localhost:5000`
- Health: `http://localhost:5000/health`
- Swagger: `http://localhost:5000/swagger`

## 6. How to run client

Verifier d'abord la configuration du frontend:

- `src/DeliveryApp.Frontend/appsettings.json`
- cle `ApiBaseUrl` (par defaut `http://localhost:5000`)

Puis lancer le client:

```bash
dotnet run --project src/DeliveryApp.Frontend --launch-profile http
```

URL frontend:

- `http://localhost:5105`

## 7. Diagram references

Ce dossier contient les diagrammes PlantUML/C4 de reference:

- `deliveryapp-c4-context.puml`
- `deliveryapp-c4-container.puml`
- `deliveryapp-class-diagram.puml`
- `deliveryapp-modular-monolith-uml.puml`
- `deliveryapp-backend-complete-class-methods.puml`
- `deliveryapp-sequence-create-order.puml`
- `deliveryapp-sequence-assign-pay-review.puml`
- `deliveryapp-order-state-machine.puml`
- `deliveryapp-data-model-erd.puml`
- `architecture-document.fr.md`
- `current-state.md`
- `modular-monolith-migration.md`

Reference principale pour la cible: `deliveryapp-modular-monolith-uml.puml`.

Pour render les diagrammes:

```bash
plantuml docs/architecture/*.puml
```
