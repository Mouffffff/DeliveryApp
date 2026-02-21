using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Application.Interfaces;

public interface IDeliveryRepository
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetByStatusAsync(int status);
    Task<Order> AddAsync(Order order);
    Task<Order?> UpdateAsync(Order order);
    Task<bool> DeleteAsync(int id);
}