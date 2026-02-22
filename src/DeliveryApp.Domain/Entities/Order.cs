using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }

    public int CustomerId { get; set; }
    public int StoreId { get; set; }
    public int DeliveryAddressId { get; set; }
    public int? CourierId { get; set; }

    public Address DeliveryAddress { get; set; } = null!;
    public List<OrderItem> OrderItems { get; set; } = new();

    public bool IsClosed => Status is OrderStatus.Delivered or OrderStatus.Cancelled;

    public void SetItems(IEnumerable<OrderItem> items)
    {
        var materialized = items.ToList();
        if (materialized.Count == 0)
        {
            throw new InvalidOperationException("Une commande doit contenir au moins un article.");
        }

        if (materialized.Any(i => i.Quantity <= 0 || i.UnitPrice <= 0))
        {
            throw new InvalidOperationException("Chaque article doit avoir une quantite et un prix unitaire strictement positifs.");
        }

        OrderItems = materialized;
        RecalculateTotal();
    }

    public void RecalculateTotal()
    {
        TotalPrice = OrderItems.Sum(i => i.UnitPrice * i.Quantity);
    }

    public bool CanTransitionTo(OrderStatus next)
    {
        if (IsClosed)
        {
            return false;
        }

        if (next == OrderStatus.Cancelled)
        {
            return true;
        }

        return next == Status || (int)next == (int)Status + 1;
    }

    public void UpdateStatus(OrderStatus next)
    {
        if (!CanTransitionTo(next))
        {
            throw new InvalidOperationException($"Transition de statut interdite: {Status} -> {next}.");
        }

        Status = next;
    }

    public void AssignCourier(int courierId)
    {
        if (courierId <= 0)
        {
            throw new InvalidOperationException("L'ID du coursier doit etre strictement positif.");
        }

        if (IsClosed)
        {
            throw new InvalidOperationException("Impossible d'affecter un coursier a une commande terminee ou annulee.");
        }

        CourierId = courierId;
        if (Status == OrderStatus.Pending)
        {
            Status = OrderStatus.Accepted;
        }
    }
}
