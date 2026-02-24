namespace DeliveryApp.Modules.Ordering.Domain.Enums;

public enum OrderStatus
{
    Pending = 0,
    Accepted = 1,
    Preparing = 2,
    ReadyForPickup = 3,
    PickedUp = 4,
    OutForDelivery = 5,
    Delivered = 6,
    Cancelled = 7
}
