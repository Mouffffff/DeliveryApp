using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Domain.Entities;

public class Courier
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public VehicleType Vehicle { get; set; }
    public bool IsAvailable { get; set; } = true;

    public List<Order> Orders { get; set; } = new();

    public void SetAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }
}
