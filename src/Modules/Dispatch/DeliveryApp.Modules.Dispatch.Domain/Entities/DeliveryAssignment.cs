using DeliveryApp.BuildingBlocks.Domain.Abstractions;

namespace DeliveryApp.Modules.Dispatch.Domain.Entities;

public sealed class DeliveryAssignment : Entity
{
    public int OrderId { get; private set; }
    public int CourierId { get; private set; }
    public DateTime AssignedOnUtc { get; private set; } = DateTime.UtcNow;
}
