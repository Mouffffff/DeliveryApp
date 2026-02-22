# DeliveryApp - Documentation Complète du Projet

## Table des Matières

1. [Présentation Générale](#présentation-générale)
2. [Architecture du Projet](#architecture-du-projet)
3. [Structure des Dossiers](#structure-des-dossiers)
4. [Technologies Utilisées](#technologies-utilisées)
5. [Modularité et Pattern DDD](#modularité-et-pattern-ddd)
6. [Guide de Démarrage](#guide-de-démarrage)

---

## Présentation Générale

**DeliveryApp** est une application backend .NET/C# pour un service de livraison de type Uber Eats, DoorDash ou Deliveroo. Elle permet la gestion complète d'un système de livraison :

- Gestion des clients et livreurs
- Gestion des restaurants/magasins et produits
- Création et suivi des commandes
- Système de paiement
- Système d'avis et de notation

L'architecture est en transition vers un **Modular Monolith** (Monolithe Modulaire), permettant une migration future vers des microservices.

---

## Architecture du Projet

### Vue d'Ensemble

```
┌─────────────────────────────────────────────────────────────────┐
│                        DeliveryApp                               │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌─────────────┐    ┌─────────────┐    ┌─────────────┐         │
│  │   Legacy    │    │BuildingBlocks│    │   Modules   │         │
│  │   Projects  │    │             │    │   (Target)  │         │
│  └─────────────┘    └─────────────┘    └─────────────┘         │
│                                                                  │
│  • Domain        • Application           • Catalog               │
│  • Application   • Domain               • Ordering              │
│  • Infrastructure│ • Infrastructure    • Dispatch              │
│  • Service                               • Payments             │
│                                          • Identity              │
└─────────────────────────────────────────────────────────────────┘
```

### Types de Projets

| Type | Description |
|------|-------------|
| **Legacy** | Architecture verticale classique (Domain/Application/Infrastructure) - encore active |
| **BuildingBlocks** | Blocs de construction réutilisables (Entity, Result, IUnitOfWork) |
| **Modules** | Nouveaux modules métier autonomes (Catalog, Ordering, Dispatch, Payments, Identity) |

---

## Structure des Dossiers

### arborescence Principale

```
src/
├── BuildingBlocks/           # Blocs de construction réutilisables
│   ├── DeliveryApp.BuildingBlocks.Application/
│   ├── DeliveryApp.BuildingBlocks.Domain/
│   └── DeliveryApp.BuildingBlocks.Infrastructure/
│
├── DeliveryApp.Api/           # Point d'entrée de l'API
│   ├── Endpoints/             # Définition des routes
│   ├── Properties/            # Configuration de lancement
│   └── appsettings.json       # Configuration
│
├── DeliveryApp.Application/   # Logique applicative (Legacy)
│   ├── Common/                # Classes communes
│   ├── DTOs/                  # Data Transfer Objects
│   ├── Interfaces/            # Interfaces des services
│   └── Validation/           # Validateurs
│
├── DeliveryApp.Domain/       # Entités et règles métier (Legacy)
│   ├── Entities/              # Objets métier
│   └── Enums/                # Énumérations
│
├── DeliveryApp.Infrastructure/# Accès données (Legacy)
│   ├── Data/                 # DbContext et Migrations
│   └── Repositories/         # Implémentations des repositories
│
├── DeliveryApp.Service/       # Services métier (Legacy)
│   └── Services/             # Implémentations des services
│
└── Modules/                   # Modules métier (Cible)
    ├── Catalog/              # Module Catalogue
    ├── Ordering/              # Module Commandes
    ├── Dispatch/              # Module Distribution/Livraison
    ├── Payments/              # Module Paiements
    └── Identity/              # Module Identité/Auth
```

---

### Détail des Dossiers

#### 1. BuildingBlocks/ - Blocs de Construction

Ces projets contiennent des abstractions réutilisables dans tous les autres projets.

```
BuildingBlocks/
├── DeliveryApp.BuildingBlocks.Domain/
│   └── Abstractions/
│       ├── Entity.cs          # Classe de base pour les entités
│       └── IDomainEvent.cs   # Interface pour les événements domaine
│
├── DeliveryApp.BuildingBlocks.Application/
│   └── Abstractions/
│       ├── Messaging.cs       # Patterns CQRS (Command/Query)
│       └── Result.cs          # Type Result pour le Railway Programming
│
└── DeliveryApp.BuildingBlocks.Infrastructure/
    └── Abstractions/
        └── IUnitOfWork.cs    # Interface Unit of Work
```

**Rôle :**
- `Entity` : Classe de base pour toutes les entités avec Id et gestion des événements domaine
- `Result<T>` : Wrapper pour gérer les succès/échecs sans exceptions
- `IUnitOfWork` : Abstraction pour la transaction database
- `Messaging` : Patterns CQRS pour commandes et requêtes

---

#### 2. DeliveryApp.Api/ - Point d'Entrée API

Projet principal qui expose les endpoints REST.

```
DeliveryApp.Api/
├── Program.cs                 # Configuration et démarrage de l'app
├── Endpoints/
│   └── DeliveryEndpoints.cs   # Définition des routes API
├── Properties/
│   └── launchSettings.json    # Configuration de lancement
├── appsettings.json           # Configuration production
└── appsettings.Development.json # Configuration développement
```

**Rôle :**
- Configure l'application ASP.NET Core
- Définit les endpoints REST
- Gère l'injection de dépendances
- Configure Swagger pour la documentation
- Gère les exceptions globales

**Endpoints disponibles :**
- `/api/customers` - Gestion des clients
- `/api/stores` - Gestion des magasins
- `/api/products` - Gestion des produits
- `/api/orders` - Gestion des commandes
- `/api/couriers` - Gestion des livreurs
- `/api/modules/*` - Nouveaux endpoints modulaires
- `/health` - Vérification de santé

---

#### 3. DeliveryApp.Domain/ - Couche Domaine (Legacy)

Contient les entités et règles métier pures.

```
DeliveryApp.Domain/
├── Entities/
│   ├── Address.cs             # Adresse de livraison
│   ├── Courier.cs             # Livreur
│   ├── Customer.cs            # Client
│   ├── Order.cs               # Commande
│   ├── OrderItem.cs           # Article d'une commande
│   ├── Payment.cs             # Paiement
│   ├── Product.cs             # Produit
│   ├── Review.cs              # Avis
│   └── Store.cs               # Magasin/Restaurant
│
└── Enums/
    ├── OrderStatus.cs         # Statuts de commande
    ├── PaymentMethod.cs       # Méthodes de paiement
    ├── PaymentStatus.cs       # Statuts de paiement
    └── VehicleType.cs         # Types de véhicule
```

**Rôle :**
- Définit les entités du domaine (objets avec identité)
- Contient les règles métier simples
- Enumérations pour les statuts et types
- **Aucune dépendance technique** (pas de EF Core, pas de logger)

**Entités :**

| Entité | Description |
|--------|-------------|
| `Customer` | Client qui passe des commandes |
| `Address` | Adresse de livraison |
| `Store` | Restaurant ou boutique |
| `Product` | Produit vendu par un store |
| `Order` | Commande passée par un client |
| `OrderItem` | Article dans une commande |
| `Courier` | Livreur qui livre les commandes |
| `Payment` | Paiement d'une commande |
| `Review` | Avis laissé par un client |

---

#### 4. DeliveryApp.Application/ - Couche Application (Legacy)

Contient la logique applicative, les DTOs et les interfaces.

```
DeliveryApp.Application/
├── Common/
│   └── ServiceResult.cs      # Résultat de service (legacy)
│
├── DTOs/
│   ├── AssignCourierDto.cs
│   ├── CommerceDtos.cs
│   ├── CreateDeliveryDto.cs
│   ├── DeliveryDto.cs
│   ├── ManagementDtos.cs
│   └── UpdateDeliveryDto.cs
│
├── Interfaces/
│   ├── ICatalogRepository.cs
│   ├── ICatalogService.cs
│   ├── ICourierRepository.cs
│   ├── ICourierService.cs
│   ├── ICustomerRepository.cs
│   ├── ICustomerService.cs
│   ├── IOrderRepository.cs
│   ├── IOrderService.cs
│   ├── IPaymentAppService.cs
│   ├── IPaymentRepository.cs
│   ├── IReviewRepository.cs
│   └── IReviewService.cs
│
└── Validation/
    ├── CatalogValidator.cs
    ├── CourierValidator.cs
    ├── CustomerValidator.cs
    ├── OrderValidator.cs
    ├── PaymentValidator.cs
    └── ReviewValidator.cs
```

**Rôle :**
- `DTOs` : Objets de transfert de données entre les couches
- `Interfaces` : Contrats (abstractions) pour les services et repositories
- `Validation` : Règles de validation des données entrantes
- `Common` : Classes utilitaires communes

---

#### 5. DeliveryApp.Infrastructure/ - Couche Infrastructure (Legacy)

Implémente l'accès aux données avec Entity Framework Core.

```
DeliveryApp.Infrastructure/
├── Data/
│   ├── AppDbContext.cs        # Contexte EF Core
│   └── Migrations/            # Migrations EF Core
│       ├── 20260220165725_InitialCreate.cs
│       ├── 20260221140832_InitialSeedData.cs
│       └── 20260221150419_CleanUpSeeding.cs
│
└── Repositories/
    ├── CatalogRepository.cs
    ├── CourierRepository.cs
    ├── CustomerRepository.cs
    ├── OrderRepository.cs
    ├── PaymentRepository.cs
    └── ReviewRepository.cs
```

**Rôle :**
- `AppDbContext` : Configuration EF Core (mappings, relations)
- `Migrations` : Scripts de création/mise à jour de la DB
- `Repositories` : Implémentations des interfaces de données

**Base de données :** SQL Server (localhost:1433)

---

#### 6. DeliveryApp.Service/ - Services Métier (Legacy)

Implémentation des services métier.

```
DeliveryApp.Service/
└── Services/
    ├── CatalogService.cs
    ├── CourierService.cs
    ├── CustomerService.cs
    ├── OrderService.cs
    ├── PaymentAppService.cs
    ├── ReviewService.cs
    └── Helpers/
        └── DeliveryDtoMapper.cs
```

**Rôle :**
- Contient la logique métier complexe
- Utilise les repositories pour accéder aux données
- Retourne des `ServiceResult<T>` (legacy)

---

#### 7. Modules/ - Architecture Modulaire (Cible)

Les modules représentent la cible future de l'architecture.

```
Modules/
├── Catalog/                   # Module Catalogue
│   ├── DeliveryApp.Modules.Catalog.Application/
│   ├── DeliveryApp.Modules.Catalog.Domain/
│   └── DeliveryApp.Modules.Catalog.Infrastructure/
│
├── Ordering/                  # Module Commandes
│   ├── DeliveryApp.Modules.Ordering.Application/
│   ├── DeliveryApp.Modules.Ordering.Domain/
│   └── DeliveryApp.Modules.Ordering.Infrastructure/
│
├── Dispatch/                  # Module Distribution
│   ├── DeliveryApp.Modules.Dispatch.Application/
│   ├── DeliveryApp.Modules.Dispatch.Domain/
│   ├── DeliveryApp.Modules.Dispatch.Infrastructure/
│   └── DeliveryApp.Modules.Dispatch.Domain/Enums/
│
├── Payments/                 # Module Paiements
│   ├── DeliveryApp.Modules.Payments.Application/
│   ├── DeliveryApp.Modules.Payments.Domain/
│   └── DeliveryApp.Modules.Payments.Infrastructure/
│
└── Identity/                 # Module Identité
    ├── DeliveryApp.Modules.Identity.Application/
    ├── DeliveryApp.Modules.Identity.Domain/
    └── DeliveryApp.Modules.Identity.Infrastructure/
```

**Structure de chaque module :**

```
Module XXX/
├── XXX.Application/          # Couche Application du module
│   └── Abstractions/        # Interfaces et DTOs
│
├── XXX.Domain/              # Couche Domaine du module
│   └── Entities/           # Entités spécifiques
│
└── XXX.Infrastructure/      # Couche Infrastructure
    └── DependencyInjection.cs # Configuration DI
```

**Rôle de chaque module :**

| Module | Responsabilité |
|--------|----------------|
| **Catalog** | Gestion des stores et produits |
| **Ordering** | Gestion des commandes |
| **Dispatch** | Affectation des livreurs |
| **Payments** | Traitement des paiements |
| **Identity** | Authentification et utilisateurs |

---

#### 8. docs/ - Documentation

```
docs/
├── architecture/
│   ├── README.md
│   ├── current-state.md
│   ├── modular-monolith-migration.md
│   ├── deliveryapp-c4-context.puml
│   ├── deliveryapp-c4-container.puml
│   ├── deliveryapp-modular-monolith-uml.puml
│   ├── deliveryapp-backend-complete-class-methods.puml
│   ├── deliveryapp-data-model-erd.puml
│   ├── deliveryapp-order-state-machine.puml
│   ├── deliveryapp-sequence-create-order.puml
│   └── deliveryapp-sequence-assign-pay-review.puml
│
├── api/
│   ├── workflow-scenario.md    # Scénario backend
│   ├── frontend-workflow.md    # Scénario frontend
│   └── simulation-checklist.md
│
└── sql/
    ├── README.md
    ├── 01_seed_reference_data.sql
    └── 02_simulation_scenario.sql
```

---

## Technologies Utilisées

| Technologie | Version | Usage |
|-------------|---------|-------|
| **.NET** | 10.0 | Framework principal |
| **ASP.NET Core** | 10.0 | API REST |
| **Entity Framework Core** | Latest | ORM pour SQL Server |
| **SQL Server** | - | Base de données |
| **Swagger/OpenAPI** | - | Documentation API |
| **FluentValidation** | - | Validation |
| **PlantUML** | - | Diagrammes UML |

---

## Modularité et Pattern DDD

### Pourquoi le Modular Monolith ?

1. **Séparation des préoccupations** : Chaque module est autonome
2. **Migration progressive** : Les modules peuvent être extraits en microservices
3. **Couplage faible** : Communication par interfaces
4. **Testabilité** : Chaque module peut être testé isolément

### Pattern DDD (Domain-Driven Design)

- **Entités** : Objets avec identité propre (Order, Customer, etc.)
- **Value Objects** : Objets immuables sans identité
- **Domain Services** : Logique métier complexe
- **Application Services** : Orchestration des use cases
- **Repositories** : Abstraction du stockage

---

## Guide de Démarrage

### Prérequis

- .NET 10.0 SDK
- SQL Server (ou Docker)

### Lancer le projet

```
bash
# Restoration des packages
dotnet restore

# Lancer l'API
dotnet run --project src/DeliveryApp.Api
```

### Accéder à l'application

- API : http://localhost:5000
- Swagger : http://localhost:5000/swagger/index.html
- Health Check : http://localhost:5000/health

### Base de données

La chaîne de connexion est dans `appsettings.json` :

```
json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=DeliveryAppDb;User Id=sa;Password=Estiam@MD_2026_ALB;TrustServerCertificate=True;"
  }
}
```

---

##Diagramme de Flux des Données

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│   Client     │────▶│     API      │────▶│  Application │
│  (Frontend)  │     │ (Web)        │     │   Services   │
└──────────────┘     └──────────────┘     └──────────────┘
                                                    │
                                                    ▼
                     ┌──────────────┐     ┌──────────────┐
                     │    SQL       │◀────│Infrastructure│
                     │  Database    │     │Repositories  │
                     └──────────────┘     └──────────────┘
```

---

## Statuts des Commandes

| Statut | Valeur | Description |
|--------|--------|-------------|
| Pending | 0 | En attente |
| Accepted | 1 | Acceptée |
| Preparing | 2 | En préparation |
| Ready | 3 | Prête |
| PickedUp | 4 | Récupérée par le livreur |
| Delivered | 5 | Livrée |
| Cancelled | 6 | Annulée |

---

## Routes API Principales

### Endpoints Legacy

| Méthode | Route | Description |
|---------|-------|-------------|
| GET | `/api/stores` | Liste des restaurants |
| GET | `/api/stores/{id}/products` | Produits d'un restaurant |
| POST | `/api/orders` | Créer une commande |
| GET | `/api/orders/{id}` | Détails d'une commande |
| PATCH | `/api/orders/{id}/assign-courier` | Assigner un livreur |
| POST | `/api/orders/{id}/payments` | Créer un paiement |
| POST | `/api/orders/{id}/reviews` | Laisser un avis |

### Endpoints Modules

| Méthode | Route | Description |
|---------|-------|-------------|
| GET | `/api/modules/catalog/stores` | Catalogue (Module) |
| POST | `/api/modules/ordering/orders` | Commandes (Module) |
| PATCH | `/api/modules/dispatch/orders/{orderId}/couriers/{courierId}` | Distribution (Module) |
| POST | `/api/modules/payments/pay` | Paiement (Module) |

---

## Prochaines Étapes

1. **Finaliser la migration** : Remplacer les endpoints legacy par les endpoints modules
2. **Supprimer les projets legacy** : Une fois la migration faite
3. **Ajouter l'authentification** : Implémenter le module Identity
4. **Créer le frontend** : Application React/Vue/Angular

---

*Document généré automatiquement pour le projet DeliveryApp*
