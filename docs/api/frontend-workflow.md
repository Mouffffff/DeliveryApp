# DeliveryApp - Scénario Frontend (Parcours Utilisateur)

Ce document décrit le parcours utilisateur de l'application DeliveryApp du point de vue du frontend.

---

## Acteurs

| Rôle | Description |
|------|-------------|
| **Client** | Utilisateur qui commande des produits |
| **Livreur** | Personne qui livre les commandes |
| **Magasin** | Restaurant/boutique qui vend des produits |
| **Admin** | Gère l'application (création stores, livreurs) |

---

## Parcours Client

### 1. Inscription / Connexion
```
┌─────────────────────────────────┐
│         Page d'accueil          │
│                                 │
│   [Se connecter] [S'inscrire]   │
│                                 │
└─────────────────────────────────┘
```

### 2. Page d'accueil (Aprè connexion)
```
┌─────────────────────────────────┐
│  DeliveryApp    [Mon Profil]   │
├─────────────────────────────────┤
│                                 │
│  🔍 Rechercher un restaurant   │
│                                 │
│  ─── Catégories ───            │
│  [Pizza] [Burger] [Sushi]      │
│  [Asian] [Indian] [FastFood]   │
│                                 │
│  ─── Restaurants ───           │
│  ┌─────┐ ┌─────┐ ┌─────┐     │
│  │     │ │     │ │     │     │
│  │ 🍕  │ │ 🍔  │ │ 🍣  │     │
│  │     │ │     │ │     │     │
│  └─────┘ └─────┘ └─────┘     │
│                                 │
└─────────────────────────────────┘
```

### 3. Page Restaurant (Menu)
```
┌─────────────────────────────────┐
│  ← Retour    Le Gourmet        │
├─────────────────────────────────┤
│  ┌─────────────────────────┐   │
│  │       PHOTO             │   │
│  │    Restaurant          │   │
│  │    ⭐ 4.5 (200 avis)   │   │
│  └─────────────────────────┘   │
│                                 │
│  ─── Menu ───                  │
│                                 │
│  🍔 Burgers                     │
│  ├── Classic Burger    12.99€  │
│  │   [+]                         │
│  ├── Cheese Burger     14.99€  │
│  │   [+]                         │
│  └── BBQ Burger        15.99€  │
│      [+]                         │
│                                 │
│  🍟 Accompagnements             │
│  ├── Frites            4.99€   │
│  │   [+]                         │
│  └── Onion Rings       5.99€   │
│      [+]                         │
│                                 │
│  ─── Boissons ───               │
│  ├── Coca-Cola         2.50€   │
│  └── Eau               1.50€   │
│                                 │
├─────────────────────────────────┤
│  🛒 Panier (3 articles)        │
│  Total: 32.97€  [Commander]    │
└─────────────────────────────────┘
```

### 4. Panier
```
┌─────────────────────────────────┐
│  ← Retour         Mon Panier    │
├─────────────────────────────────┤
│                                 │
│  ┌─────────────────────────┐   │
│  │ Classic Burger    1x   │   │
│  │                     12.99€│   │
│  │              [−] [+]    │   │
│  └─────────────────────────┘   │
│                                 │
│  ┌─────────────────────────┐   │
│  │ Frites           1x    │   │
│  │                      4.99€│   │
│  │              [−] [+]    │   │
│  └─────────────────────────┘   │
│                                 │
│  ┌─────────────────────────┐   │
│  │ Coca-Cola         1x   │   │
│  │                      2.50€│   │
│  │              [−] [+]    │   │
│  └─────────────────────────┘   │
│                                 │
│  ───                           │
│  Sous-total:        20.48€      │
│  Frais livraison:   5.00€      │
│  Service:           2.05€      │
│  ───                           │
│  TOTAL:              27.53€    │
│                                 │
│  ─── Livrer à ───              │
│  📍 123 Rue de la Paix        │
│  [Changer l'adresse]           │
│                                 │
├─────────────────────────────────┤
│         [Passer la commande]    │
│              27.53€            │
└─────────────────────────────────┘
```

### 5. Confirmation de Commande
```
┌─────────────────────────────────┐
│                                 │
│           ✅                   │
│     Commande confirmée!        │
│                                 │
│  ─── Détails ───               │
│  Commande #1234                 │
│  Le Gourmet                    │
│                                 │
│  Livré à:                       │
│  123 Rue de la Paix            │
│                                 │
│  📦 Statut: En préparation     │
│                                 │
│  ───                           │
│  ⏱️ Temps estimé: 35 min       │
│                                 │
│  [Suivre ma commande]           │
│  [Retour à l'accueil]          │
│                                 │
└─────────────────────────────────┘
```

### 6. Suivi de Commande
```
┌─────────────────────────────────┐
│  ← Retour    Suivi Commande    │
├─────────────────────────────────┤
│                                 │
│  Commande #1234    27.53€      │
│                                 │
│  ─── Tracking ───              │
│                                 │
│  ✅ Commande passée             │
│     14:30                      │
│        │                       │
│        ▼                       │
│  🔄 En préparation             │
│     14:35                      │
│        │                       │
│        ▼                       │
│  📦 Prête pour ramassage       │
│        │                       │
│        ▼                       │
│  🚴 En livraison               │
│        │                       │
│        ▼                       │
│  🏠 Livrée                     │
│                                 │
│  ─── Livreur ───               │
│  👤 Marie Martin               │
│  🚴 Vélo                      │
│  📞 06 12 34 56 78            │
│                                 │
│  ─── Détails ───               │
│  2x Classic Burger             │
│  1x Frites                     │
│  1x Coca-Cola                  │
│                                 │
└─────────────────────────────────┘
```

### 7. Avis sur la Commande
```
┌─────────────────────────────────┐
│  ← Retour    Avis              │
├─────────────────────────────────┤
│                                 │
│  Comment était votre commande?  │
│                                 │
│  ⭐⭐⭐⭐⭐                    │
│     [Cliquez pour noter]       │
│                                 │
│  ───                           │
│                                 │
│  Laissez un commentaire:       │
│  ┌─────────────────────────┐   │
│  │                         │   │
│  │                         │   │
│  │                         │   │
│  └─────────────────────────┘   │
│                                 │
├─────────────────────────────────┤
│         [Envoyer l'avis]        │
└─────────────────────────────────┘
```

---

## Parcours Admin / Gestionnaire

### 1. Dashboard Admin
```
┌─────────────────────────────────┐
│  Admin Panel    [Déconnexion]  │
├─────────────────────────────────┤
│                                 │
│  📊 Aperçu du jour             │
│  ├── Commandes: 45             │
│  ├── En cours: 12              │
│  ├── Livrées: 33              │
│  └── Revenus: 1,245.00€       │
│                                 │
│  ─── Menu ───                  │
│  [📦 Commandes]                │
│  [🏪 Magasins]                 │
│  [🚴 Livreurs]                 │
│  [👥 Clients]                  │
│  [📈 Statistiques]             │
│                                 │
└─────────────────────────────────┘
```

### 2. Gestion des Commandes
```
┌─────────────────────────────────┐
│  ← Retour   Commandes         │
├─────────────────────────────────┤
│                                 │
│  [Filtres: Toutes ▼]           │
│                                 │
│  ┌─────────────────────────┐   │
│  │ #1234  Le Gourmet      │   │
│  │ Client: Jean D.        │   │
│  │ Statut: En préparation │   │
│  │ [Détails] [Assigner]   │   │
│  └─────────────────────────┘   │
│                                 │
│  ┌─────────────────────────┐   │
│  │ #1233  Sushi Zen       │   │
│  │ Client: Marie L.       │   │
│  │ Statut: Livrée         │   │
│  │ [Détails]              │   │
│  └─────────────────────────┘   │
│                                 │
│  ┌─────────────────────────┐   │
│  │ #1232  Pizza Italia    │   │
│  │ Client: Pierre P.      │   │
│  │ Statut: Annulée        │   │
│  │ [Détails]              │   │
│  └─────────────────────────┘   │
│                                 │
└─────────────────────────────────┘
```

### 3. Assigner un Livreur
```
┌─────────────────────────────────┐
│  ← Retour   Assigner Livreur   │
├─────────────────────────────────┤
│                                 │
│  Commande #1234                 │
│  Le Gourmet                     │
│                                 │
│  ─── Livreurs disponibles ───  │
│                                 │
│  ┌─────────────────────────┐   │
│  │ 👤 Marie Martin        │   │
│  │ 🚴 Vélo                │   │
│  │ À 2.5 km              │   │
│  │ ⭐ 4.8                │   │
│  │    [Assigner]          │   │
│  └─────────────────────────┘   │
│                                 │
│  ┌─────────────────────────┐   │
│  │ 👤 Jean Dupont         │   │
│  │ 🚴 Scooter             │   │
│  │ À 3.1 km              │   │
│  │ ⭐ 4.6                │   │
│  │    [Assigner]          │   │
│  └─────────────────────────┘   │
│                                 │
│  ┌─────────────────────────┐   │
│  │ 👤 Sophie Bernard       │   │
│  │ 🚴 Voiture             │   │
│  │ À 5.0 km              │   │
│  │ ⭐ 4.9                │   │
│  │    [Assigner]          │   │
│  └─────────────────────────┘   │
│                                 │
└─────────────────────────────────┘
```

---

## Parcours Livreur

### 1. Liste des Courses
```
┌─────────────────────────────────┐
│  Livreur: Marie    [Déconnexion]│
├─────────────────────────────────┤
│                                 │
│  ─── À livrer (3) ───          │
│                                 │
│  ┌─────────────────────────┐   │
│  │ #1234                   │   │
│  │ Le Gourmet              │   │
│  │ 📍 2.5 km              │   │
│  │ [Accepter]             │   │
│  └─────────────────────────┘   │
│                                 │
│  ┌─────────────────────────┐   │
│  │ #1235                   │   │
│  │ Sushi Zen               │   │
│  │ 📍 3.0 km              │   │
│  │ [Accepter]             │   │
│  └─────────────────────────┘   │
│                                 │
│  ─── En cours (2) ───          │
│                                 │
│  ┌─────────────────────────┐   │
│  │ #1230 - En livraison    │   │
│  │ 📍 1.2 km - Marie       │   │
│  │ [Livré]                 │   │
│  └─────────────────────────┘   │
│                                 │
└─────────────────────────────────┘
```

### 2. Détails de la Course
```
┌─────────────────────────────────┐
│  ← Retour   Course #1234       │
├─────────────────────────────────┤
│                                 │
│  ─── Client ───                │
│  Jean Dupont                   │
│  📞 06 12 34 56 78            │
│                                 │
│  ─── Adresse ───              │
│  123 Rue de la Paix           │
│  75001 Paris                  │
│                                 │
│  ─── Commande ───             │
│  2x Classic Burger             │
│  1x Frites                    │
│  1x Coca-Cola                  │
│                                 │
│  ─── Restaurant ───           │
│  Le Gourmet                    │
│  45 Rue du Restaurant         │
│                                 │
├─────────────────────────────────┤
│     [Confirmer le ramassage]    │
└─────────────────────────────────┘
```

---

## Flux Global Simplifié

```
┌──────────┐     ┌──────────┐     ┌──────────┐
│  Client  │────▶│Frontend  │────▶│  Backend │
└──────────┘     └──────────┘     └──────────┘
     │                │                │
     │  1. S'inscrire│                │
     │◀───────────────│                │
     │                │ POST /customers│
     │                │───────────────▶│
     │                │                │
     │  2. Parcourir │                │
     │◀───────────────│                │
     │                │ GET /stores    │
     │                │───────────────▶│
     │                │                │
     │  3. Commander │                │
     │◀───────────────│                │
     │                │ POST /orders   │
     │                │───────────────▶│
     │                │                │
     │  4. Payer     │                │
     │◀───────────────│                │
     │                │ POST /payments │
     │                │───────────────▶│
     │                │                │
     │  5. Suivre    │                │
     │◀───────────────│                │
     │                │ GET /orders/.. │
     │                │───────────────▶│
     │                │                │
     │  6. Livrer    │                │
     │                │                │◀─── Admin/Livreur
     │                │                │
     │  7. Avis      │                │
     │◀───────────────│                │
     │                │ POST /reviews  │
     │                │───────────────▶│
```

---

## Écrans à Implémenter

| Écran | Route | Description |
|--------|-------|-------------|
| Login | `/login` | Connexion utilisateur |
| Register | `/register` | Inscription |
| Home | `/` | Page d'accueil avec restaurants |
| Restaurant | `/restaurant/:id` | Menu d'un restaurant |
| Cart | `/cart` | Panier |
| Checkout | `/checkout` | Validation commande |
| OrderConfirmation | `/order/:id/confirmation` | Confirmation |
| OrderTracking | `/order/:id/track` | Suivi en temps réel |
| OrderHistory | `/orders` | Historique des commandes |
| OrderDetails | `/order/:id` | Détails d'une commande |
| Review | `/order/:id/review` | Laisser un avis |
| Profile | `/profile` | Profil utilisateur |
| AdminDashboard | `/admin` | Dashboard admin |
| AdminOrders | `/admin/orders` | Gestion commandes |
| AdminStores | `/admin/stores` | Gestion restaurants |
| AdminCouriers | `/admin/couriers` | Gestion livreurs |
| CourierDashboard | `/courier` | Dashboard livreur |
| CourierDelivery | `/courier/delivery/:id` | Détails course |

---

## Données à Afficher par Écran

### Home
- Liste des catégories
- Liste des restaurants avec:
  - Nom, photo
  - Note (étoiles)
  - Temps de livraison moyen
  - Frais de livraison

### Restaurant
- Photo de couverture
- Nom, description
- Note et nombre d'avis
- Catégories de produits
- Liste des produits avec:
  - Nom, description
  - Prix
  - Bouton "Ajouter"

### Panier
- Liste des articles avec_quantité
- Prix unitaire
- Sous-total
- Frais de livraison
- Total
- Adresse de livraison
- Bouton "Commander"

### Suivi Commande
- Timeline des statuts
- Position du livreur (optionnel)
- Infos livreur (nom, téléphone)
- Détails de la commande

### Admin - Commandes
- Tableau des commandes
- Filtres par statut
- Actions: voir détails, assigner livreur, annuler
