using DeliveryApp.Application.Interfaces;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _db;

    public PaymentRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Payment>> GetPaymentsByOrderIdAsync(int orderId) =>
        _db.Payments
            .Where(p => p.OrderId == orderId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();

    public async Task<Payment> AddPaymentAsync(Payment payment)
    {
        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();
        return payment;
    }
}
