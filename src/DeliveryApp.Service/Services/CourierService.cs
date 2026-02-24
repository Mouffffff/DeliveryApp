using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Application.Validation;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Enums;
using DeliveryApp.Service.Services.Helpers;

namespace DeliveryApp.Service;

public class CourierService : ICourierService
{
    private readonly ICourierRepository _couriers;
    private readonly IOrderRepository _orders;

    public CourierService(ICourierRepository couriers, IOrderRepository orders)
    {
        _couriers = couriers;
        _orders = orders;
    }

    public async Task<IReadOnlyList<CourierDto>> GetCouriersAsync()
    {
        var couriers = await _couriers.GetCouriersAsync();
        return couriers.Select(c => new CourierDto(c.Id, c.FullName, c.PhoneNumber, c.Vehicle.ToString())).ToList();
    }

    public async Task<IReadOnlyList<CourierDto>> GetAvailableCouriersAsync()
    {
        var couriers = await _couriers.GetAvailableCouriersAsync();
        return couriers.Select(c => new CourierDto(c.Id, c.FullName, c.PhoneNumber, c.Vehicle.ToString())).ToList();
    }

    public async Task<ServiceResult<CourierDto>> CreateCourierAsync(CreateCourierDto dto)
    {
        var validation = CourierValidator.ValidateCreate(dto);
        if (!validation.ok)
        {
            return ServiceResult<CourierDto>.Failure("validation_error", validation.error, 400);
        }

        var created = await _couriers.AddCourierAsync(new Courier
        {
            FullName = dto.FullName.Trim(),
            PhoneNumber = dto.PhoneNumber.Trim(),
            Vehicle = (VehicleType)dto.Vehicle,
            IsAvailable = true
        });

        return ServiceResult<CourierDto>.Success(new CourierDto(
            created.Id,
            created.FullName,
            created.PhoneNumber,
            created.Vehicle.ToString()));
    }

    public async Task<ServiceResult<DeliveryDto>> AssignCourierAsync(int orderId, int courierId)
    {
        if (orderId <= 0)
        {
            return ServiceResult<DeliveryDto>.Failure("invalid_order_id", "L'ID de commande doit etre superieur a 0.", 400);
        }

        if (courierId <= 0)
        {
            return ServiceResult<DeliveryDto>.Failure("invalid_courier_id", "L'ID du coursier doit etre superieur a 0.", 400);
        }

        var order = await _orders.GetByIdAsync(orderId);
        if (order is null)
        {
            return ServiceResult<DeliveryDto>.Failure("order_not_found", "Commande introuvable.", 404);
        }

        if (order.CourierId.HasValue && order.CourierId.Value != courierId)
        {
            return ServiceResult<DeliveryDto>.Failure("courier_already_assigned", "La commande a deja un autre coursier affecte.", 409);
        }

        var courier = await _couriers.GetCourierByIdAsync(courierId);
        if (courier is null)
        {
            return ServiceResult<DeliveryDto>.Failure("courier_not_found", "Coursier introuvable.", 404);
        }

        if (!courier.IsAvailable && order.CourierId != courierId)
        {
            return ServiceResult<DeliveryDto>.Failure("courier_unavailable", "Ce coursier n'est pas disponible.", 409);
        }

        try
        {
            order.AssignCourier(courierId);
        }
        catch (InvalidOperationException ex)
        {
            return ServiceResult<DeliveryDto>.Failure("invalid_order_operation", ex.Message, 409);
        }

        var updated = await _orders.UpdateAsync(order);
        if (updated is null)
        {
            return ServiceResult<DeliveryDto>.Failure("assign_failed", "L'affectation du coursier a echoue.", 500);
        }

        courier.SetAvailability(false);
        await _couriers.SetCourierAvailabilityAsync(courierId, courier.IsAvailable);
        return ServiceResult<DeliveryDto>.Success(DeliveryDtoMapper.ToDeliveryDto(updated));
    }
}
