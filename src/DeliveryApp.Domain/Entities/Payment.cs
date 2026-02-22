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

    public static Payment CreateCompleted(int orderId, decimal amount, PaymentMethod method, DateTime paymentDateUtc)
    {
        if (orderId <= 0)
        {
            throw new InvalidOperationException("L'ID de commande doit etre strictement positif.");
        }

        if (amount <= 0)
        {
            throw new InvalidOperationException("Le montant du paiement doit etre strictement positif.");
        }

        return new Payment
        {
            OrderId = orderId,
            Amount = amount,
            Method = method,
            Status = PaymentStatus.Completed,
            PaymentDate = paymentDateUtc
        };
    }
}
