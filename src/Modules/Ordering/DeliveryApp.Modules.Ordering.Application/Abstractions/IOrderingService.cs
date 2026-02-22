using DeliveryApp.BuildingBlocks.Application.Abstractions;

namespace DeliveryApp.Modules.Ordering.Application.Abstractions;

public interface IOrderingService
{
    Task<Result<OrderDetails>> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken cancellationToken = default);
    Task<Result<OrderDetails>> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default);
}

public sealed record PlaceOrderLine(int ProductId, int Quantity);
public sealed record PlaceOrderRequest(int CustomerId, int StoreId, int DeliveryAddressId, IReadOnlyList<PlaceOrderLine> Items);
public sealed record OrderDetails(int Id, string Status, decimal TotalPrice, DateTimeOffset CreatedAt);
