using System;

namespace DeliveryApp.Application.DTOs;

public record DeliveryItemDto(
    int ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal
);

public record DeliveryDto(
    int Id,
    int CustomerId,
    int StoreId,
    int DeliveryAddressId,
    decimal TotalPrice,
    string Status,
    DateTimeOffset CreatedAt,
    IReadOnlyList<DeliveryItemDto> Items,
    DateTimeOffset? CompletedAt
);
