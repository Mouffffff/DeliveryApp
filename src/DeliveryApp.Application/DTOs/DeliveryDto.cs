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
    string StoreName,
    string StoreStreet,
    string StoreCity,
    string StoreZipCode,
    string CustomerName,
    string CustomerPhoneNumber,
    int? CourierId,
    string? CourierName,
    string? CourierPhoneNumber,
    string? CourierVehicle,
    string DeliveryStreet,
    string DeliveryCity,
    string DeliveryZipCode,
    DateTimeOffset CreatedAt,
    IReadOnlyList<DeliveryItemDto> Items,
    DateTimeOffset? CompletedAt
);
