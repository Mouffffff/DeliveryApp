using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Application.Interfaces;

public interface ICourierRepository
{
    Task<List<Courier>> GetCouriersAsync();
    Task<List<Courier>> GetAvailableCouriersAsync();
    Task<Courier?> GetCourierByIdAsync(int courierId);
    Task SetCourierAvailabilityAsync(int courierId, bool isAvailable);
    Task<Courier> AddCourierAsync(Courier courier);
}
