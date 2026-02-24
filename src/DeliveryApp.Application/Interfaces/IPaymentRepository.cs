using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Application.Interfaces;

public interface IPaymentRepository
{
    Task<List<Payment>> GetPaymentsByOrderIdAsync(int orderId);
    Task<Payment> AddPaymentAsync(Payment payment);
}
