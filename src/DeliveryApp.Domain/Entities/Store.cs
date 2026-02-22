namespace DeliveryApp.Domain.Entities;

public class Store
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public int AddressId { get; set; }
    public Address Location { get; set; } = null!;

    public List<Product> Products { get; set; } = new();

    public static Store Create(string name, string category, int addressId)
    {
        var normalizedName = name?.Trim() ?? string.Empty;
        var normalizedCategory = category?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            throw new InvalidOperationException("Le nom du magasin est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(normalizedCategory))
        {
            throw new InvalidOperationException("La categorie du magasin est obligatoire.");
        }

        if (addressId <= 0)
        {
            throw new InvalidOperationException("L'adresse du magasin est invalide.");
        }

        return new Store
        {
            Name = normalizedName,
            Category = normalizedCategory,
            AddressId = addressId
        };
    }
}
