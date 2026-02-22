using DeliveryApp.BuildingBlocks.Domain.Abstractions;

namespace DeliveryApp.Modules.Ordering.Domain.Entities;

public sealed class OrderItem : Entity
{
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
}
