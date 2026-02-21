namespace DeliveryApp.Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    
    // Utile pour une future intégration GPS/Maps
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}