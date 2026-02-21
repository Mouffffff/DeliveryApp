namespace DeliveryApp.Domain.Entities;

public class Review
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    
    public int Rating { get; set; } // Score de 1 à 5
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}