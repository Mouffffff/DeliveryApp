using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
}