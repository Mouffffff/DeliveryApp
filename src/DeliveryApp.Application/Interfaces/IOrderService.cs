using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Interfaces;

public interface IOrderService
{
    Task<IReadOnlyList<DeliveryDto>> GetAllAsync();
    Task<ServiceResult<DeliveryDto>> GetByIdAsync(int id);
    Task<ServiceResult<IReadOnlyList<DeliveryDto>>> GetByStatusAsync(int status);
    Task<ServiceResult<DeliveryDto>> CreateAsync(CreateDeliveryDto dto);
    Task<ServiceResult<DeliveryDto>> UpdateAsync(int id, UpdateDeliveryDto dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}
