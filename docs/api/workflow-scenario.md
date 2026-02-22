# DeliveryApp - Scénario de Fonctionnement

Ce document décrit le flux complet d'une commande de livraison dans l'application DeliveryApp.

## Architecture des Endpoints

L'API propose deux styles d'endpoints :
1. **Endpoints Legacy** : Routes classiques dans `/api/*`
2. **Endpoints Modules** : Nouvelles routes modulaires dans `/api/modules/*`

---

## Scénario Complet d'une Commande

### Phase 1 : Initialisation (Setup)

Ces étapes sont généralement effectuées par l'administrateur ou lors de la configuration initiale.

| # | Action | Endpoint | Méthode |
|---|--------|----------|---------|
| 1 | Créer un client | `/api/customers` | POST |
| 2 | Créer une adresse de livraison | `/api/addresses` | POST |
| 3 | Créer un magasin (restaurant/boutique) | `/api/stores` | POST |
| 4 | Créer des produits pour le magasin | `/api/products` | POST |
| 5 | Créer un livreur (courier) | `/api/couriers` | POST |

### Phase 2 : Consultation (Par le client)

| # | Action | Endpoint | Méthode |
|---|--------|----------|---------|
| 6 | Liste des magasins disponibles | `/api/stores` | GET |
| 7 | Produits d'un magasin spécifique | `/api/stores/{storeId}/products` | GET |
| 8 | Liste des livreurs disponibles | `/api/couriers/available` | GET |

### Phase 3 : Passation de Commande

| # | Action | Endpoint | Méthode |
|---|--------|----------|---------|
| 9 | Créer une nouvelle commande | `/api/orders` | POST |

**Exemple de payload pour créer une commande :**
```
json
{
  "customerId": 1,
  "storeId": 1,
  "deliveryAddressId": 1,
  "items": [
    { "productId": 1, "quantity": 2 },
    { "productId": 3, "quantity": 1 }
  ]
}
```

### Phase 4 : Gestion de la Commande

| # | Action | Endpoint | Méthode |
|---|--------|----------|---------|
| 10 | Affecter un livreur à la commande | `/api/orders/{orderId}/assign-courier` | PATCH |
| 11 | Voir les détails d'une commande | `/api/orders/{id}` | GET |
| 12 | Liste de toutes les commandes | `/api/orders` | GET |
| 13 | Commandes par statut | `/api/orders/status/{status}` | GET |

### Phase 5 : Paiement

| # | Action | Endpoint | Méthode |
|---|--------|----------|---------|
| 14 | Créer un paiement pour la commande | `/api/orders/{orderId}/payments` | POST |
| 15 | Voir les paiements d'une commande | `/api/orders/{orderId}/payments` | GET |

### Phase 6 : Avis (Optionnel)

| # | Action | Endpoint | Méthode |
|---|--------|----------|---------|
| 16 | Donner un avis sur la commande | `/api/orders/{orderId}/reviews` | POST |
| 17 | Voir les avis d'une commande | `/api/orders/{orderId}/reviews` | GET |

---

## Nouveaux Endpoints (Modules)

Les modules offrent une approche plus structurée et moderne.

### Catalog Module
```
GET /api/modules/catalog/stores                          - Liste des magasins
GET /api/modules/catalog/stores/{storeId}/products       - Produits d'un magasin
```

### Ordering Module
```
POST /api/modules/ordering/orders                        - Passer une commande
GET  /api/modules/ordering/orders/{orderId}             - Détails d'une commande
```

### Dispatch Module
```
PATCH /api/modules/dispatch/orders/{orderId}/couriers/{courierId}  - Assigner un livreur
GET  /api/modules/dispatch/couriers/available            - Livreurs disponibles
```

### Payments Module
```
POST /api/modules/payments/pay                          - Paiement d'une commande
GET  /api/modules/payments/orders/{orderId}             - Paiements d'une commande
```

---

## Statuts des Commandes (OrderStatus)

Les statuts possibles d'une commande :

| Statut | Valeur | Description |
|--------|--------|-------------|
| Pending | 0 | Commande créée, en attente de traitement |
| Accepted | 1 | Commande acceptée par le magasin |
| Preparing | 2 | Le magasin prépare la commande |
| Ready | 3 | Commande prête pour le ramassage |
| PickedUp | 4 | Livreur a récupéré la commande |
| Delivered | 5 | Commande livrée au client |
| Cancelled | 6 | Commande annulée |

---

## Statuts des Paiements (PaymentStatus)

| Statut | Valeur | Description |
|--------|--------|-------------|
| Pending | 0 | Paiement en attente |
| Completed | 1 | Paiement réussi |
| Failed | 2 | Paiement échoué |
| Refunded | 3 | Paiement remboursé |

---

## Méthodes de Paiement (PaymentMethod)

| Méthode | Description |
|---------|-------------|
| CreditCard | Carte de crédit |
| DebitCard | Carte de débit |
| Cash | Espèces |
| DigitalWallet | Portefeuille digital |

---

## Exemple de Flux Complet

```
1. Client s'inscrit
   POST /api/customers
   → { id: 1, name: "Jean Dupont" }

2. Client ajoute son adresse
   POST /api/addresses
   → { id: 1, street: "123 Rue de la Paix", city: "Paris" }

3. Admin crée un restaurant
   POST /api/stores
   → { id: 1, name: "Le Gourmet" }

4. Admin ajoute des produits
   POST /api/products
   → { id: 1, name: "Burger", price: 12.99 }
   → { id: 2, name: "Frites", price: 4.99 }

5. Admin crée un livreur
   POST /api/couriers
   → { id: 1, name: "Marie Martin", vehicleType: "Bike" }

6. Client consulte les restaurants
   GET /api/stores
   → [{ id: 1, name: "Le Gourmet" }]

7. Client voit les produits
   GET /api/stores/1/products
   → [{ id: 1, name: "Burger", price: 12.99 }, ...]

8. Client passe commande
   POST /api/orders
   → { id: 1, status: "Pending", totalPrice: 29.97 }

9. Admin affecte un livreur
   PATCH /api/orders/1/assign-courier
   → { id: 1, courierId: 1, status: "Accepted" }

10. Client paie la commande
    POST /api/orders/1/payments
    → { id: 1, status: "Completed", amount: 29.97 }

11. Client donne son avis
    POST /api/orders/1/reviews
    → { id: 1, rating: 5, comment: "Excellent service!" }
```

---

## Notes

- Tous les endpoints retournent des codes HTTP standard (200, 201, 400, 404, 500)
- Les endpoints POST et PUT/PATCH utilisent JSON pour les données
- La base de données doit être种子ée (seed) avec des données de test
- Le Swagger est disponible sur `/swagger/index.html` pour tester les endpoints
