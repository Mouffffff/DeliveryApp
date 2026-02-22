using DeliveryApp.BuildingBlocks.Application.Abstractions;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Modules.Ordering.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryApp.Modules.Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderingModule(this IServiceCollection services)
    {
        services.AddScoped<IOrderingService, OrderingServiceAdapter>();
        return services;
    }
}

internal sealed class OrderingServiceAdapter : IOrderingService
{
    private readonly IOrderService _orders;

    public OrderingServiceAdapter(IOrderService orders)
    {
        _orders = orders;
    }

    public async Task<Result<OrderDetails>> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken cancellationToken = default)
    {
        var dto = new CreateDeliveryDto
        {
            CustomerId = request.CustomerId,
            StoreId = request.StoreId,
            DeliveryAddressId = request.DeliveryAddressId,
            Items = request.Items.Select(i => new CreateDeliveryItemDto(i.ProductId, i.Quantity)).ToList()
        };

        var result = await _orders.CreateAsync(dto);
        if (!result.IsSuccess || result.Value is null)
        {
            return Result<OrderDetails>.Failure(result.Error?.Message ?? "Order creation failed.");
        }

        return Result<OrderDetails>.Success(ToOrderDetails(result.Value));
    }

    public async Task<Result<OrderDetails>> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var result = await _orders.GetByIdAsync(orderId);
        if (!result.IsSuccess || result.Value is null)
        {
            return Result<OrderDetails>.Failure(result.Error?.Message ?? "Order not found.");
        }

        return Result<OrderDetails>.Success(ToOrderDetails(result.Value));
    }

    private static OrderDetails ToOrderDetails(DeliveryDto order) =>
        new(order.Id, order.Status, order.TotalPrice, order.CreatedAt);
}
