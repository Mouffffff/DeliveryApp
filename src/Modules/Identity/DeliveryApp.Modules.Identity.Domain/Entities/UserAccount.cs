using DeliveryApp.BuildingBlocks.Domain.Abstractions;
using DeliveryApp.Modules.Identity.Domain.Enums;

namespace DeliveryApp.Modules.Identity.Domain.Entities;

public sealed class UserAccount : Entity
{
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public AppRole Role { get; private set; } = AppRole.Customer;
    public bool IsActive { get; private set; } = true;
}
