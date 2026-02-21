namespace DeliveryApp.Domain.Entities;

public class Store
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // ex: "Fast Food", "Supermarché"

    // Adresse physique du magasin
    public int AddressId { get; set; }
    public Address Location { get; set; } = null!;

    // Catalogue de produits du magasin
    public List<Product> Products { get; set; } = new();
}