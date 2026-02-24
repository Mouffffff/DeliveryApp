namespace DeliveryApp.Frontend.Models;

public sealed record HealthResponse(string Status, DateTimeOffset Timestamp);

public sealed record StoreDto(
    int Id,
    string Name,
    string Category,
    int AddressId,
    string Street,
    string City,
    string ZipCode
);

public sealed record ProductDto(
    int Id,
    string Name,
    decimal Price,
    int StoreId
);

public sealed record CourierDto(
    int Id,
    string FullName,
    string PhoneNumber,
    string Vehicle
);

public sealed record CustomerDto(
    int Id,
    string FullName,
    string Email,
    string PhoneNumber
);

public sealed record AddressDto(
    int Id,
    string Street,
    string City,
    string ZipCode,
    int? CustomerId
);

public sealed record DeliveryItemDto(
    int ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal
);

public sealed record DeliveryDto(
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

public sealed record PaymentDto(
    int Id,
    int OrderId,
    decimal Amount,
    string Method,
    string Status,
    DateTimeOffset PaymentDate
);

public sealed record ReviewDto(
    int Id,
    int OrderId,
    int Rating,
    string Comment,
    DateTimeOffset CreatedAt
);

public sealed record CreateOrderItemRequest(int ProductId, int Quantity);

public sealed class CreateOrderRequest
{
    public int CustomerId { get; set; }
    public int StoreId { get; set; }
    public List<int> ProductIds { get; set; } = new();
    public List<CreateOrderItemRequest> Items { get; set; } = new();
    public int DeliveryAddressId { get; set; }
}

public sealed record AssignCourierRequest(int CourierId);

public sealed record UpdateOrderRequest(int Status, int? CourierId);

public sealed record CreatePaymentRequest(int Method);

public sealed record CreateReviewRequest(int Rating, string Comment);

public sealed record CreateAddressRequest(string Street, string City, string ZipCode, int? CustomerId);

public sealed record CreateStoreRequest(string Name, string Category, int AddressId);

public sealed record UpdateStoreRequest(string Name, string Category, int AddressId);

public sealed record SetAccountStatusRequest(bool IsActive);

public sealed record AdminUserAccountDto(
    int Id,
    string Email,
    string DisplayName,
    AppRoleCode Role,
    bool IsActive,
    int? CustomerId,
    int? CourierId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? LastLoginAt);

public sealed record LoginRequest(string Email, string Password);

public sealed record RegisterCustomerAccountRequest(
    string FullName,
    string Email,
    string PhoneNumber,
    string Password,
    string Street,
    string City,
    string ZipCode);

public sealed record RegisterCourierAccountRequest(
    string FullName,
    string Email,
    string PhoneNumber,
    int Vehicle,
    string Password);

public sealed record RegisterAdminAccountRequest(
    string FullName,
    string Email,
    string Password,
    string? BootstrapKey);

public sealed record AuthTokenResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    int UserId,
    string Email,
    string DisplayName,
    AppRoleCode Role,
    int? CustomerId,
    int? CourierId);

public sealed record AccountContextResponse(
    string Role,
    int? CustomerId,
    int? CourierId,
    CustomerDto? Customer,
    IReadOnlyList<AddressDto> Addresses);

public enum OrderStatusCode
{
    Pending = 0,
    Accepted = 1,
    Preparing = 2,
    ReadyForPickup = 3,
    PickedUp = 4,
    OutForDelivery = 5,
    Delivered = 6,
    Cancelled = 7
}

public enum PaymentMethodCode
{
    CreditCard = 0,
    PayPal = 1,
    CashOnDelivery = 2,
    ApplePay = 3,
    GooglePay = 4
}

public enum AppRoleCode
{
    Customer = 0,
    Courier = 1,
    Admin = 2
}
