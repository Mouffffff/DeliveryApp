namespace DeliveryApp.Modules.Payments.Domain.Enums;

public enum PaymentStatus
{
    Unpaid = 0,
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Refunded = 4
}
