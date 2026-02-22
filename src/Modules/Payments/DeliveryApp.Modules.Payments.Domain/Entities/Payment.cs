using DeliveryApp.BuildingBlocks.Domain.Abstractions;
using DeliveryApp.Modules.Payments.Domain.Enums;

namespace DeliveryApp.Modules.Payments.Domain.Entities;

public sealed class Payment : Entity
{
    public int OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime PaidOnUtc { get; private set; } = DateTime.UtcNow;
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; }
}
