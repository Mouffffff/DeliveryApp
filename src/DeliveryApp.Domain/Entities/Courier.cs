using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Domain.Entities;

public class Courier
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    // Utilise l'Enum VehicleType que tu as déjà créé
    public VehicleType Vehicle { get; set; }
    
    // Pour savoir si le livreur peut prendre une commande
    public bool IsAvailable { get; set; } = true;

    // Propriété de navigation : Liste des commandes livrées par ce coursier
    public List<Order> Orders { get; set; } = new();
}