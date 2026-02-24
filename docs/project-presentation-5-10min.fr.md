# DeliveryApp - Script de presentation (5 a 10 minutes)

## Comment utiliser ce fichier
- C'est un script de presentation, pas un README.
- Chaque section correspond a une slide.
- Vise 40-60 secondes par slide.

---

## Slide 1 - Introduction du projet
**Message a dire :**
DeliveryApp est une plateforme de livraison de repas inspiree de Uber Eats.  
Elle est construite autour de trois acteurs : Customer, Courier et Admin.  
L'objectif principal etait pragmatique : livrer un produit fonctionnel de bout en bout, puis optimiser l'architecture et la qualite.

---

## Slide 2 - Objectifs du projet
**Message a dire :**
Le projet repose sur cinq objectifs principaux :
1. Construire un workflow de livraison complet, de la creation de commande jusqu'a l'avis final.
2. Appliquer une securite par roles et des regles metier robustes via des API securisees.
3. Separer clairement les responsabilites dans le backend.
4. Garder une architecture evolutive avec une trajectoire modular monolith.
5. Fournir un frontend vraiment utilisable, pas seulement une simulation API.

---

## Slide 3 - Ce que l'application fait aujourd'hui
**Message a dire :**
Perimetre fonctionnel actuel :
- Customer : consulter stores/produits, commander, suivre, payer, noter.
- Courier : voir les commandes disponibles, s'affecter, faire avancer les statuts.
- Admin : gerer stores/produits/adresses et moderer les comptes.

Donc on n'est pas sur une simple preuve technique : la plateforme est fonctionnelle.

---

## Slide 4 - Vue architecture
**Message a dire :**
Architecture globale :
- Frontend Blazor avec experience par role.
- API ASP.NET Core Minimal API comme point d'entree.
- Couches Application et Service pour les cas d'usage et validations.
- Couche Infrastructure avec repositories EF Core.
- Base relationnelle SQL Server.

Cette architecture en couches garde la logique metier claire et le comportement API previsible.

---

## Slide 5 - Choix d'architecture
**Message a dire :**
Decisions importantes :
- Authentification JWT avec policies par role (`CustomerOnly`, `CourierOnly`, `AdminOnly`).
- Services/repositories specialises a la place d'un service/repository geant.
- Runtime legacy stable aujourd'hui, avec migration progressive via `/api/modules/*`.

Resultat : le systeme est operationnel maintenant et reste evolutif.

---

## Slide 6 - Vue du data model
**Message a dire :**
Entites principales :
- `Customers`, `Addresses`
- `Stores`, `Products`
- `Orders`, `OrderItems`
- `Couriers`
- `Payments`, `Reviews`
- `UserAccounts` (auth + role identity)

L'entite centrale est `Orders`, qui relie client, store, adresse de livraison et eventuellement un livreur.

---

## Slide 7 - Integrite et regles metier du data model
**Message a dire :**
Contraintes et regles cle :
- `UserAccounts.Email` est unique.
- Le total d'une commande vient des lignes `OrderItems`.
- Les transitions de statut sont controlees et sequentielles.
- Le montant de paiement doit correspondre au total de commande.
- Un seul avis par commande, apres livraison.
- L'acces aux ressources est controle par role et ownership.

Ces regles garantissent la coherence des donnees et la fiabilite metier.

---

## Slide 8 - Workflow de demo de bout en bout
**Message a dire :**
Scenario de demo :
1. Login en Customer.
2. Creation d'une commande depuis un store.
3. Passage en Courier pour affectation et progression de statut.
4. Retour en Customer pour voir les infos du livreur (nom, telephone, vehicule).
5. Paiement et avis.
6. Passage en Admin pour montrer la gestion.

Ce scenario demontre la coordination entre roles sur un cycle complet.

---

## Slide 9 - Etat actuel du projet
**Message a dire :**
Aujourd'hui, le projet est operationnel :
- Backend operationnel.
- Frontend operationnel.
- Modele de donnees et scripts SQL en place.
- Workflow principal commande -> livraison -> paiement -> avis disponible.

L'objectif principal est atteint : une application fonctionnelle.

---

## Slide 10 - Prochaines etapes et conclusion
**Message a dire :**
Prochaines priorites :
1. Ajouter des tests automatises (integration + end-to-end).
2. Finaliser la migration des endpoints legacy vers les endpoints modules.
3. Renforcer la readiness production (secrets, monitoring, CI/CD).
4. Ameliorer l'experience utilisateur et le temps reel.

**Phrase de conclusion :**
DeliveryApp est deja une application utilisable, avec une base technique solide et une feuille de route claire vers un niveau production.
