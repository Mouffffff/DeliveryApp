# DeliveryApp - Documentation Complete

## 1. Vision du projet

DeliveryApp est une application de livraison type Uber Eats, avec:

- un backend ASP.NET Core (API REST + JWT + roles)
- un frontend Blazor Server (application role-based)
- une base SQL Server

Le repo combine deux dimensions:

- `Legacy runtime`: couches Domain/Application/Infrastructure/Service actuellement en production dans le code
- `Target architecture`: modular monolith (`Modules/*` + `BuildingBlocks/*`) pour migration progressive

## 2. Fonctionnalites couvertes

### Cote Customer

- inscription et connexion JWT
- consultation des stores et produits
- ajout panier
- creation commande
- suivi de ses commandes
- paiement
- avis

### Cote Courier

- inscription et connexion JWT
- consultation board commandes
- affectation commande
- progression des statuts de livraison

### Cote Admin

- inscription bootstrap admin + connexion JWT
- gestion des stores (creation, modification, suppression)
- gestion des adresses de stores
- moderation des comptes (blocage/deblocage)
- supervision commandes
- acces complet d'administration

## 3. Architecture technique

## 3.1 Projets principaux (runtime actif)

- `src/DeliveryApp.Api`: endpoints HTTP, auth JWT, policies, swagger
- `src/DeliveryApp.Application`: DTOs, interfaces, validations, `ServiceResult`
- `src/DeliveryApp.Domain`: entites metier et enums
- `src/DeliveryApp.Infrastructure`: EF Core, `AppDbContext`, repositories SQL
- `src/DeliveryApp.Service`: services metier
- `src/DeliveryApp.Frontend`: UI Blazor role-based

## 3.2 Projets de cible modular monolith

- `src/BuildingBlocks/*`: abstractions partagees (`Result`, `Messaging`, `Entity`, `IUnitOfWork`)
- `src/Modules/Catalog/*`
- `src/Modules/Ordering/*`
- `src/Modules/Dispatch/*`
- `src/Modules/Payments/*`
- `src/Modules/Identity/*`

Important:
- les routes `/api/modules/*` passent par des adapters et reutilisent encore les services legacy
- `Modules.Identity` contient actuellement un stub non branche sur l'auth runtime

## 4. Stack technique

- .NET 10 (`net10.0`)
- ASP.NET Core Minimal APIs
- Entity Framework Core 10 + SQL Server
- JWT Bearer Authentication
- Swagger / OpenAPI
- Blazor Server interactive

## 5. Structure du repository

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

## 6. Modele de donnees (principal)

Tables metier:

- `Customers`
- `Addresses`
- `Stores`
- `Products`
- `Couriers`
- `Orders`
- `OrderItems`
- `Payments`
- `Reviews`
- `UserAccounts` (auth JWT)

Contraintes notables:

- `UserAccounts.Email` unique
- `UserAccounts.CustomerId` unique filtre non null
- `UserAccounts.CourierId` unique filtre non null
- relation `Orders -> Addresses` en `Restrict`
- relation `OrderItems -> Orders` en `Cascade`

## 7. Authentification et autorisation

## 7.1 JWT

Configuration API:

- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:Secret` (minimum 32 caracteres)
- `Jwt:AccessTokenMinutes`

Claims utilises:

- `sub`, `name`, `email`, `role`
- `customerId` (si role Customer)
- `courierId` (si role Courier)

## 7.2 Roles

- `Customer`
- `Courier`
- `Admin`

Policies declarees:

- `CustomerOnly`
- `CourierOnly`
- `AdminOnly`
- `CourierOrAdmin`

## 7.3 Bootstrap admin

Quand un admin existe deja, l'endpoint de creation admin exige la cle:

- `Auth:AdminBootstrapKey`

Valeur par defaut actuelle (dev):

- `deliveryapp-bootstrap-admin-2026`

## 8. Endpoints API

Base URL en profil `http`:

- `http://localhost:5000`

Health:

- `GET /health` (public)

Auth:

- `POST /api/auth/login` (public)
- `POST /api/auth/register/customer` (public)
- `POST /api/auth/register/courier` (public)
- `POST /api/auth/register/admin` (public, bootstrap key selon cas)
- `GET /api/auth/me` (auth requis)

Compte:

- `GET /api/account/context` (auth requis)
- `GET /api/account/addresses` (`CustomerOnly`)
- `POST /api/account/addresses` (`CustomerOnly`)

Gestion referentiel (admin):

- `GET /api/customers` (`AdminOnly`)
- `POST /api/addresses` (`AdminOnly`)
- `GET /api/addresses` (`AdminOnly`)
- `POST /api/stores` (`AdminOnly`)
- `PUT /api/stores/{storeId}` (`AdminOnly`)
- `DELETE /api/stores/{storeId}` (`AdminOnly`)
- `POST /api/products` (`AdminOnly`)
- `GET /api/auth/admin/accounts` (`AdminOnly`)
- `PATCH /api/auth/admin/accounts/{accountId}/status` (`AdminOnly`)

Consultation:

- `GET /api/stores` (public)
- `GET /api/stores/{storeId}/products` (public)
- `GET /api/couriers` (`CourierOrAdmin`)
- `GET /api/couriers/available` (`CourierOrAdmin`)

Commandes:

- `GET /api/orders/mine` (auth requis, filtre par role)
- `GET /api/orders` (`CourierOrAdmin`)
- `GET /api/orders/{id}` (auth requis, controle d'acces par role)
- `GET /api/orders/status/{status}` (`CourierOrAdmin`)
- `POST /api/orders` (`CustomerOnly`)
- `PUT /api/orders/{id}` (`CourierOnly`)
- `PATCH /api/orders/{orderId}/assign-courier` (`CourierOnly`)
- `DELETE /api/orders/{id}` (`AdminOnly`)

Paiements:

- `POST /api/orders/{orderId}/payments` (`CustomerOnly`, avec controle d'acces commande)
- `GET /api/orders/{orderId}/payments` (auth requis, avec controle d'acces commande)

Avis:

- `POST /api/orders/{orderId}/reviews` (`CustomerOnly`, avec controle d'acces commande)
- `GET /api/orders/{orderId}/reviews` (auth requis, avec controle d'acces commande)

Routes modules (`/api/modules/*`):

- `GET /api/modules/catalog/stores` (`AdminOnly`)
- `GET /api/modules/catalog/stores/{storeId}/products` (`AdminOnly`)
- `POST /api/modules/ordering/orders` (`CustomerOnly`)
- `GET /api/modules/ordering/orders/{orderId}` (auth requis)
- `PATCH /api/modules/dispatch/orders/{orderId}/couriers/{courierId}` (`CourierOnly`)
- `GET /api/modules/dispatch/couriers/available` (`CourierOnly`)
- `POST /api/modules/payments/pay` (`CustomerOnly`)
- `GET /api/modules/payments/orders/{orderId}` (auth requis)

## 9. Regles metier principales

Commandes:

- minimum 1 article
- quantites strictement positives
- tous les produits doivent appartenir au store de la commande
- progression statut pas a pas (`N -> N+1`) ou annulation
- commande fermee (`Delivered` ou `Cancelled`) non modifiable

Dispatch:

- un courier indisponible ne peut pas etre affecte a une nouvelle commande
- affecter un courier sur commande `Pending` passe le statut en `Accepted`

Paiement:

- montant doit etre egal au total de commande
- un seul paiement `Completed` par commande

Avis:

- autorise uniquement si commande `Delivered`
- un seul avis par commande

## 10. Frontend Blazor (usage role-based)

Base URL en profil `http`:

- `http://localhost:5105`

Session:

- token JWT conserve dans `AuthSessionState` (scope Blazor Server)
- `DeliveryApiClient` ajoute automatiquement `Authorization: Bearer ...`

Acces pages par role:

- Public: `/account`, `/forbidden`, `/not-found`
- Customer: `/stores`, `/stores/{id}`, `/cart`, `/my-orders`
- Courier: `/orders/board`, `/orders`, `/orders/{id}`
- Admin: `/admin`, `/dashboard`, `/orders/{id}` (consultation)

Workflow principal:

1. connexion/inscription via `/account`
2. customer -> selection store -> panier -> checkout
3. creation commande -> suivi
4. courier -> affectation + avancement statut
5. customer -> paiement + avis

## 11. Installation et lancement

Prerequis:

- .NET SDK 10
- SQL Server accessible (ex: `localhost,1433`)
- Azure Data Studio ou SSMS pour scripts SQL

Depuis la racine du repo:

```bash
dotnet restore
```

## 11.1 Base de donnees

Option A (recommandee): migrations EF Core

```bash
dotnet ef database update --project src/DeliveryApp.Infrastructure/DeliveryApp.Infrastructure.csproj --startup-project src/DeliveryApp.Api/DeliveryApp.Api.csproj
```

Option B: base deja existante

- verifier que le schema contient toutes les tables attendues
- executer `docs/sql/03_auth_user_accounts.sql` si `UserAccounts` absent

Seed de reference (ordre conseille):

1. `docs/sql/01_seed_reference_data.sql`
2. `docs/sql/02_simulation_scenario.sql` (optionnel, jeu d'exemple)

## 11.2 Lancer API et Front

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

## 12. Procedure de test rapide

## 12.1 Test API (Swagger/Postman)

1. creer ou connecter un compte via `/api/auth/*`
2. recuperer le `accessToken`
3. injecter `Bearer <token>` dans les requetes securisees
4. tester selon role:
- Customer: commande + paiement + avis
- Courier: board + assign + update statut
- Admin: administration complete + supervision

## 12.2 Test Front

1. ouvrir `http://localhost:5105/account`
2. se connecter ou s'inscrire
3. suivre navigation selon role
4. verifier la coherence des donnees entre UI et API

## 13. SQL utilitaire auth

Script:

- `docs/sql/03_auth_user_accounts.sql`

But:

- creation table `UserAccounts` + contraintes + index
- verification rapide via `SELECT TOP 5 * FROM dbo.UserAccounts`

## 14. Gestion des erreurs

Format reponse:

- `Results.Problem(...)` avec `title`, `detail`, `statusCode`
- extension `errorCode` pour les erreurs metier

Codes frequents:

- `400`: validation
- `401`: identifiants invalides / token invalide
- `403`: acces refuse / bootstrap admin interdit
- `404`: ressource introuvable
- `409`: conflit metier
- `500`: erreur serveur inattendue

## 15. Points d'attention actuels

- des projets modules existent mais la logique coeur reste majoritairement dans les services legacy
- `Modules.Identity` n'est pas encore le moteur auth runtime
- pas de suite de tests automatises dans le repo (integration/e2e a ajouter)
- secrets de dev visibles dans `appsettings*.json`: a externaliser avant prod

## 16. Documentation complementaire

- `docs/runbook-backend.md`
- `docs/sql/README.md`
- `docs/api/frontend-workflow.md`
- `docs/api/workflow-scenario.md`
- `docs/api/usage-checklist.md`
- `docs/architecture/README.md`
- `docs/architecture/current-state.md`
- `docs/architecture/modular-monolith-migration.md`
- diagrammes PlantUML dans `docs/architecture/*.puml`

## 17. Definition "backend operationnel"

Le backend est considere operationnel si:

- migrations/schema OK
- endpoints auth JWT fonctionnels
- policies role appliquees correctement
- creation et suivi commande OK
- paiement + avis conformes aux regles metier
- front Blazor connecte et utilisable par role

Ce statut est atteint en environnement local de developpement, avec SQL seed et deux processus (`API` + `Frontend`) demarres.
