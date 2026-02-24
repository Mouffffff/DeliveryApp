using DeliveryApp.BuildingBlocks.Domain.Abstractions;
using DeliveryApp.Modules.Dispatch.Domain.Enums;

namespace DeliveryApp.Modules.Dispatch.Domain.Entities;

public sealed class Courier : Entity
{
    public string FullName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public VehicleType Vehicle { get; private set; }
    public bool IsAvailable { get; private set; } = true;
}
