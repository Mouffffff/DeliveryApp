namespace DeliveryApp.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public static OrderItem Create(int productId, int quantity, decimal unitPrice)
    {
        if (productId <= 0)
        {
            throw new InvalidOperationException("L'ID produit est invalide.");
        }

        if (quantity <= 0)
        {
            throw new InvalidOperationException("La quantite doit etre strictement positive.");
        }

        if (unitPrice <= 0)
        {
            throw new InvalidOperationException("Le prix unitaire doit etre strictement positif.");
        }

        return new OrderItem
        {
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
}
