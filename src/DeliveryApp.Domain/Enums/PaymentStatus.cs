namespace DeliveryApp.Domain.Enums;

public enum PaymentStatus
{
    Unpaid,     // En attente de paiement
    Pending,    // Transaction en cours de vérification
    Completed,  // Payé avec succès
    Failed,     // Le paiement a été rejeté
    Refunded    // Argent rendu au client
}