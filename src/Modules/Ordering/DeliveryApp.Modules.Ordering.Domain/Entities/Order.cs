using DeliveryApp.BuildingBlocks.Domain.Abstractions;
using DeliveryApp.Modules.Ordering.Domain.Enums;

namespace DeliveryApp.Modules.Ordering.Domain.Entities;

public sealed class Order : Entity
{
    public int CustomerId { get; private set; }
    public int StoreId { get; private set; }
    public int DeliveryAddressId { get; private set; }
    public int? CourierId { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal TotalPrice { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
}
