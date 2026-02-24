namespace DeliveryApp.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    public int StoreId { get; set; }
    public Store Store { get; set; } = null!;

    public static Product Create(string name, decimal price, int storeId)
    {
        var normalizedName = name?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            throw new InvalidOperationException("Le nom du produit est obligatoire.");
        }

        if (price <= 0)
        {
            throw new InvalidOperationException("Le prix du produit doit etre superieur a 0.");
        }

        if (storeId <= 0)
        {
            throw new InvalidOperationException("Le magasin du produit est invalide.");
        }

        return new Product
        {
            Name = normalizedName,
            Price = price,
            StoreId = storeId
        };
    }
}
