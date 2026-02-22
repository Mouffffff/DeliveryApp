namespace DeliveryApp.Application.DTOs;

public record StoreDto(
    int Id,
    string Name,
    string Category,
    string Street,
    string City,
    string ZipCode
);

public record ProductDto(
    int Id,
    string Name,
    decimal Price,
    int StoreId
);

public record CourierDto(
    int Id,
    string FullName,
    string PhoneNumber,
    string Vehicle
);

public record CreatePaymentDto(
    decimal Amount,
    int Method
);

public record PaymentDto(
    int Id,
    int OrderId,
    decimal Amount,
    string Method,
    string Status,
    DateTimeOffset PaymentDate
);
