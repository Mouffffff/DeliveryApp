using Microsoft.EntityFrameworkCore;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Infrastructure.Data;

namespace DeliveryApp.Infrastructure.Repositories;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly AppDbContext _db;

    public DeliveryRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _db.Orders
            .Include(o => o.DeliveryAddress) // Inclut l'adresse de livraison
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _db.Orders
            .Include(o => o.DeliveryAddress)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetByStatusAsync(int status)
    {
        // On filtre en castant l'enum OrderStatus en int
        return await _db.Orders
            .Include(o => o.DeliveryAddress)
            .Where(o => (int)o.Status == status)
            .ToListAsync();
    }

    public async Task<Order> AddAsync(Order order)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> UpdateAsync(Order order)
    {
        var existing = await _db.Orders.FindAsync(order.Id);
        
        if (existing == null) return null;

        // Mise à jour selon les propriétés de ta classe Order
        existing.OrderDate = order.OrderDate;
        existing.TotalPrice = order.TotalPrice;
        existing.Status = order.Status;
        existing.CustomerId = order.CustomerId;
        existing.StoreId = order.StoreId;
        existing.DeliveryAddressId = order.DeliveryAddressId;
        existing.CourierId = order.CourierId;

        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _db.Orders.FindAsync(id);
        
        if (existing == null) return false;

        _db.Orders.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}