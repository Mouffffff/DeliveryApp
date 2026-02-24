using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Interfaces;

public interface ICourierService
{
    Task<IReadOnlyList<CourierDto>> GetCouriersAsync();
    Task<IReadOnlyList<CourierDto>> GetAvailableCouriersAsync();
    Task<ServiceResult<CourierDto>> CreateCourierAsync(CreateCourierDto dto);
    Task<ServiceResult<DeliveryDto>> AssignCourierAsync(int orderId, int courierId);
}
