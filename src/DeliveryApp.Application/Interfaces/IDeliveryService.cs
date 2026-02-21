using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Interfaces;

public interface IDeliveryService
{
    Task<List<DeliveryDto>> GetAllAsync();
    
    Task<DeliveryDto?> GetByIdAsync(int id);
    
    Task<List<DeliveryDto>> GetByStatusAsync(int status);

    Task<(bool ok, string error, DeliveryDto? created)> 
        CreateAsync(CreateDeliveryDto dto);

    Task<(bool ok, string error, DeliveryDto? updated)> 
        UpdateAsync(int id, UpdateDeliveryDto dto);

    Task<bool> DeleteAsync(int id);
}