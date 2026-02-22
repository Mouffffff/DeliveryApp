using DeliveryApp.Application.Interfaces;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Enums;
using DeliveryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Order>> GetAllAsync() =>
        _db.Orders
            .Include(o => o.DeliveryAddress)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

    public Task<Order?> GetByIdAsync(int id) =>
        _db.Orders
            .Include(o => o.DeliveryAddress)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

    public Task<List<Order>> GetByStatusAsync(OrderStatus status) =>
        _db.Orders
            .Include(o => o.DeliveryAddress)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

    public async Task<Order> AddAsync(Order order)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> UpdateAsync(Order order)
    {
        var existing = await _db.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        if (existing is null)
        {
            return null;
        }

        existing.Status = order.Status;
        existing.CourierId = order.CourierId;
        await _db.SaveChangesAsync();
        return await GetByIdAsync(existing.Id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (existing is null)
        {
            return false;
        }

        _db.Orders.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}
