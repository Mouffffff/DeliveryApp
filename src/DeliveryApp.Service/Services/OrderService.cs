using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Application.Validation;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Enums;
using DeliveryApp.Service.Services.Helpers;

namespace DeliveryApp.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orders;
    private readonly ICatalogRepository _catalog;
    private readonly ICustomerRepository _customers;
    private readonly ICourierRepository _couriers;

    public OrderService(
        IOrderRepository orders,
        ICatalogRepository catalog,
        ICustomerRepository customers,
        ICourierRepository couriers)
    {
        _orders = orders;
        _catalog = catalog;
        _customers = customers;
        _couriers = couriers;
    }

    public async Task<IReadOnlyList<DeliveryDto>> GetAllAsync() =>
        (await _orders.GetAllAsync()).Select(DeliveryDtoMapper.ToDeliveryDto).ToList();

    public async Task<ServiceResult<DeliveryDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResult<DeliveryDto>.Failure("invalid_id", "L'ID de commande doit etre superieur a 0.", 400);
        }

        var item = await _orders.GetByIdAsync(id);
        if (item is null)
        {
            return ServiceResult<DeliveryDto>.Failure("order_not_found", "Commande introuvable.", 404);
        }

        return ServiceResult<DeliveryDto>.Success(DeliveryDtoMapper.ToDeliveryDto(item));
    }

    public async Task<ServiceResult<IReadOnlyList<DeliveryDto>>> GetByStatusAsync(int status)
    {
        if (!Enum.IsDefined(typeof(OrderStatus), status))
        {
            return ServiceResult<IReadOnlyList<DeliveryDto>>.Failure("invalid_status", "Le statut fourni est invalide.", 400);
        }

        var items = await _orders.GetByStatusAsync((OrderStatus)status);
        return ServiceResult<IReadOnlyList<DeliveryDto>>.Success(items.Select(DeliveryDtoMapper.ToDeliveryDto).ToList());
    }

    public async Task<ServiceResult<DeliveryDto>> CreateAsync(CreateDeliveryDto dto)
    {
        var validation = OrderValidator.ValidateCreate(dto);
        if (!validation.ok)
        {
            return ServiceResult<DeliveryDto>.Failure("validation_error", validation.error, 400);
        }

        if (!await _customers.CustomerExistsAsync(dto.CustomerId))
        {
            return ServiceResult<DeliveryDto>.Failure("customer_not_found", "Le client n'existe pas.", 404);
        }

        if (!await _catalog.StoreExistsAsync(dto.StoreId))
        {
            return ServiceResult<DeliveryDto>.Failure("store_not_found", "Le magasin n'existe pas.", 404);
        }

        if (!await _customers.AddressExistsAsync(dto.DeliveryAddressId))
        {
            return ServiceResult<DeliveryDto>.Failure("address_not_found", "L'adresse de livraison n'existe pas.", 404);
        }

        var normalizedItems = NormalizeItems(dto);
        var requestedProductIds = normalizedItems.Select(i => i.ProductId).Distinct().ToList();
        var products = await _catalog.GetProductsByIdsAsync(requestedProductIds);

        if (products.Count != requestedProductIds.Count)
        {
            return ServiceResult<DeliveryDto>.Failure("unknown_products", "Un ou plusieurs produits sont introuvables.", 400);
        }

        if (products.Any(p => p.StoreId != dto.StoreId))
        {
            return ServiceResult<DeliveryDto>.Failure("invalid_products_for_store", "Tous les produits doivent appartenir au magasin de la commande.", 409);
        }

        var productMap = products.ToDictionary(p => p.Id);
        var orderItems = normalizedItems
            .Select(i => OrderItem.Create(i.ProductId, i.Quantity, productMap[i.ProductId].Price))
            .ToList();

        var entity = new Order
        {
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            CustomerId = dto.CustomerId,
            StoreId = dto.StoreId,
            DeliveryAddressId = dto.DeliveryAddressId
        };
        entity.SetItems(orderItems);

        var created = await _orders.AddAsync(entity);
        var createdWithDetails = await _orders.GetByIdAsync(created.Id);
        if (createdWithDetails is null)
        {
            return ServiceResult<DeliveryDto>.Failure("order_creation_failed", "La commande n'a pas pu etre relue apres creation.", 500);
        }

        return ServiceResult<DeliveryDto>.Success(DeliveryDtoMapper.ToDeliveryDto(createdWithDetails));
    }

    public async Task<ServiceResult<DeliveryDto>> UpdateAsync(int id, UpdateDeliveryDto dto)
    {
        if (id <= 0)
        {
            return ServiceResult<DeliveryDto>.Failure("invalid_id", "L'ID de commande doit etre superieur a 0.", 400);
        }

        var validation = OrderValidator.ValidateUpdate(dto);
        if (!validation.ok)
        {
            return ServiceResult<DeliveryDto>.Failure("validation_error", validation.error, 400);
        }

        var existing = await _orders.GetByIdAsync(id);
        if (existing is null)
        {
            return ServiceResult<DeliveryDto>.Failure("order_not_found", "Commande introuvable.", 404);
        }

        var nextStatus = (OrderStatus)dto.Status;
        try
        {
            if (dto.CourierId.HasValue)
            {
                existing.AssignCourier(dto.CourierId.Value);
            }

            existing.UpdateStatus(nextStatus);
        }
        catch (InvalidOperationException ex)
        {
            return ServiceResult<DeliveryDto>.Failure("invalid_order_operation", ex.Message, 409);
        }

        var updated = await _orders.UpdateAsync(existing);
        if (updated is null)
        {
            return ServiceResult<DeliveryDto>.Failure("update_failed", "La mise a jour de la commande a echoue.", 500);
        }

        if ((nextStatus == OrderStatus.Delivered || nextStatus == OrderStatus.Cancelled) && updated.CourierId.HasValue)
        {
            await _couriers.SetCourierAvailabilityAsync(updated.CourierId.Value, true);
        }

        return ServiceResult<DeliveryDto>.Success(DeliveryDtoMapper.ToDeliveryDto(updated));
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResult<bool>.Failure("invalid_id", "L'ID de commande doit etre superieur a 0.", 400);
        }

        var ok = await _orders.DeleteAsync(id);
        return ok
            ? ServiceResult<bool>.Success(true)
            : ServiceResult<bool>.Failure("order_not_found", "Commande introuvable.", 404);
    }

    private static List<CreateDeliveryItemDto> NormalizeItems(CreateDeliveryDto dto)
    {
        if (dto.Items is { Count: > 0 })
        {
            return dto.Items
                .GroupBy(i => i.ProductId)
                .Select(g => new CreateDeliveryItemDto(g.Key, g.Sum(i => i.Quantity)))
                .ToList();
        }

        return dto.ProductIds
            .GroupBy(productId => productId)
            .Select(g => new CreateDeliveryItemDto(g.Key, g.Count()))
            .ToList();
    }
}
