namespace DeliveryApp.Domain.Enums;

public enum OrderStatus
{
    Pending,          // Client vient de commander
    Accepted,         // Le magasin (Store) a validé la commande
    Preparing,        // En cours de préparation au magasin
    ReadyForPickup,   // Prêt à être récupéré par le livreur
    PickedUp,         // Le livreur a récupéré le colis
    OutForDelivery,   // Le livreur est sur la route vers le client
    Delivered,        // Livraison effectuée avec succès
    Cancelled         // Commande annulée
}