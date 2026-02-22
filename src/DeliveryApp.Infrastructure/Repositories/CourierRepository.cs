using DeliveryApp.Application.Interfaces;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Repositories;

public class CourierRepository : ICourierRepository
{
    private readonly AppDbContext _db;

    public CourierRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Courier>> GetCouriersAsync() =>
        _db.Couriers
            .OrderBy(c => c.FullName)
            .ToListAsync();

    public Task<List<Courier>> GetAvailableCouriersAsync() =>
        _db.Couriers
            .Where(c => c.IsAvailable)
            .OrderBy(c => c.FullName)
            .ToListAsync();

    public Task<Courier?> GetCourierByIdAsync(int courierId) =>
        _db.Couriers.FirstOrDefaultAsync(c => c.Id == courierId);

    public async Task SetCourierAvailabilityAsync(int courierId, bool isAvailable)
    {
        var courier = await _db.Couriers.FirstOrDefaultAsync(c => c.Id == courierId);
        if (courier is null)
        {
            return;
        }

        courier.IsAvailable = isAvailable;
        await _db.SaveChangesAsync();
    }

    public async Task<Courier> AddCourierAsync(Courier courier)
    {
        _db.Couriers.Add(courier);
        await _db.SaveChangesAsync();
        return courier;
    }
}
