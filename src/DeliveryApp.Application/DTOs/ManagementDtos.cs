namespace DeliveryApp.Application.DTOs;

public record CreateCustomerDto(
    string FullName,
    string Email,
    string PhoneNumber
);

public record CustomerDto(
    int Id,
    string FullName,
    string Email,
    string PhoneNumber
);

public record CreateAddressDto(
    string Street,
    string City,
    string ZipCode,
    int? CustomerId
);

public record AddressDto(
    int Id,
    string Street,
    string City,
    string ZipCode,
    int? CustomerId
);

public record CreateStoreDto(
    string Name,
    string Category,
    int AddressId
);

public record CreateProductDto(
    string Name,
    decimal Price,
    int StoreId
);

public record CreateCourierDto(
    string FullName,
    string PhoneNumber,
    int Vehicle
);

public record CreateReviewDto(
    int Rating,
    string Comment
);

public record ReviewDto(
    int Id,
    int OrderId,
    int Rating,
    string Comment,
    DateTimeOffset CreatedAt
);
