using DeliveryApp.Application.DTOs;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Service.Services.Helpers;

internal static class DeliveryDtoMapper
{
    internal static DeliveryDto ToDeliveryDto(Order order)
    {
        var items = order.OrderItems
            .Select(oi => new DeliveryItemDto(
                oi.ProductId,
                oi.Product?.Name ?? $"Product #{oi.ProductId}",
                oi.Quantity,
                oi.UnitPrice,
                oi.UnitPrice * oi.Quantity))
            .ToList();

        DateTimeOffset? completedAt = order.Status == OrderStatus.Delivered
            ? new DateTimeOffset(DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc))
            : null;

        return new DeliveryDto(
            order.Id,
            order.CustomerId,
            order.StoreId,
            order.DeliveryAddressId,
            order.TotalPrice,
            order.Status.ToString(),
            order.Store?.Name ?? $"Store #{order.StoreId}",
            order.Store?.Location?.Street ?? string.Empty,
            order.Store?.Location?.City ?? string.Empty,
            order.Store?.Location?.ZipCode ?? string.Empty,
            order.Customer?.FullName ?? $"Customer #{order.CustomerId}",
            order.Customer?.PhoneNumber ?? string.Empty,
            order.CourierId,
            order.Courier?.FullName,
            order.Courier?.PhoneNumber,
            order.Courier?.Vehicle.ToString(),
            order.DeliveryAddress?.Street ?? string.Empty,
            order.DeliveryAddress?.City ?? string.Empty,
            order.DeliveryAddress?.ZipCode ?? string.Empty,
            new DateTimeOffset(DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc)),
            items,
            completedAt);
    }

    internal static ReviewDto ToReviewDto(Review review) =>
        new(
            review.Id,
            review.OrderId,
            review.Rating,
            review.Comment,
            new DateTimeOffset(DateTime.SpecifyKind(review.CreatedAt, DateTimeKind.Utc)));
}
