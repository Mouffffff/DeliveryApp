# DeliveryApp - Scenario de fonctionnement (API actuelle)

Ce document decrit le flux complet d'une commande DeliveryApp avec les endpoints actuellement exposes par `DeliveryApp.Api`.

## 1. Styles d'endpoints

L'API expose 2 familles de routes:

1. Endpoints runtime principaux dans `/api/*`
2. Endpoints de migration modulaire dans `/api/modules/*`

Important:
- Les routes `/api/modules/*` passent encore via des adapters relies aux services legacy.
- Le moteur runtime principal reste les endpoints `/api/*`.

## 2. Prerequis du scenario

1. API lancee sur `http://localhost:5000`
2. Base SQL migree/seedee
3. Comptes disponibles:
- 1 `Admin`
- 1 `Customer`
- 1 `Courier`

## 3. Scenario nominal complet

### Phase A - Authentification

| # | Action | Endpoint | Methode | Role |
|---|---|---|---|---|
| 1 | Creer un customer | `/api/auth/register/customer` | POST | Public |
| 2 | Creer un courier | `/api/auth/register/courier` | POST | Public |
| 3 | Login | `/api/auth/login` | POST | Public |
| 4 | Lire son profil JWT | `/api/auth/me` | GET | Auth |

### Phase B - Initialisation catalogue (admin)

| # | Action | Endpoint | Methode | Role |
|---|---|---|---|---|
| 5 | Creer une adresse de store | `/api/addresses` | POST | AdminOnly |
| 6 | Creer un store | `/api/stores` | POST | AdminOnly |
| 7 | Creer des produits | `/api/products` | POST | AdminOnly |

### Phase C - Consultation et commande (customer)

| # | Action | Endpoint | Methode | Role |
|---|---|---|---|---|
| 8 | Lister les stores | `/api/stores` | GET | Public |
| 9 | Lire les produits d'un store | `/api/stores/{storeId}/products` | GET | Public |
| 10 | Lister ses adresses | `/api/account/addresses` | GET | CustomerOnly |
| 11 | Creer commande | `/api/orders` | POST | CustomerOnly |
| 12 | Suivre ses commandes | `/api/orders/mine` | GET | Auth |
| 13 | Detail commande | `/api/orders/{id}` | GET | Auth |

Payload exemple pour `POST /api/orders`:

```json
{
  "customerId": 2,
  "storeId": 1,
  "deliveryAddressId": 3,
  "items": [
    { "productId": 1, "quantity": 2 },
    { "productId": 3, "quantity": 1 }
  ]
}
```

Note:
- En role `Customer`, le backend force `customerId` depuis le token JWT.

### Phase D - Dispatch (courier)

| # | Action | Endpoint | Methode | Role |
|---|---|---|---|---|
| 14 | Voir commandes actives | `/api/orders` | GET | CourierOrAdmin |
| 15 | S'affecter une commande | `/api/orders/{orderId}/assign-courier` | PATCH | CourierOnly |
| 16 | Faire avancer le statut | `/api/orders/{id}` | PUT | CourierOnly |

Payload exemple pour `PATCH /api/orders/{orderId}/assign-courier`:

```json
{
  "courierId": 1
}
```

Payload exemple pour `PUT /api/orders/{id}` (une etape):

```json
{
  "status": 2,
  "courierId": 1
}
```

### Phase E - Paiement et avis (customer)

| # | Action | Endpoint | Methode | Role |
|---|---|---|---|---|
| 17 | Payer la commande | `/api/orders/{orderId}/payments` | POST | CustomerOnly |
| 18 | Lire historique paiements | `/api/orders/{orderId}/payments` | GET | Auth |
| 19 | Creer un avis | `/api/orders/{orderId}/reviews` | POST | CustomerOnly |
| 20 | Lire avis | `/api/orders/{orderId}/reviews` | GET | Auth |

## 4. Endpoints modules (migration progressive)

Catalog:

```text
GET /api/modules/catalog/stores
GET /api/modules/catalog/stores/{storeId}/products
```

Ordering:

```text
POST /api/modules/ordering/orders
GET  /api/modules/ordering/orders/{orderId}
```

Dispatch:

```text
PATCH /api/modules/dispatch/orders/{orderId}/couriers/{courierId}
GET  /api/modules/dispatch/couriers/available
```

Payments:

```text
POST /api/modules/payments/pay
GET  /api/modules/payments/orders/{orderId}
```

## 5. Statuts de commande (OrderStatus)

| Valeur | Statut |
|---|---|
| 0 | Pending |
| 1 | Accepted |
| 2 | Preparing |
| 3 | ReadyForPickup |
| 4 | PickedUp |
| 5 | OutForDelivery |
| 6 | Delivered |
| 7 | Cancelled |

Regles:
- progression pas a pas (`N -> N+1`)
- `Cancelled` possible tant que la commande n'est pas deja fermee
- une commande `Delivered` ou `Cancelled` ne peut plus etre modifiee

## 6. Regles metier critiques

- Tous les produits de la commande doivent appartenir au meme store.
- Un courier indisponible ne peut pas etre affecte.
- Un seul paiement `Completed` par commande.
- Un avis seulement si la commande est `Delivered`, et un seul avis par commande.

## 7. References

- `docs/api/frontend-workflow.md`
- `docs/api/usage-checklist.md`
- `docs/architecture/architecture-document.fr.md`
