using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Application.DTOs;

public record LoginRequestDto(
    string Email,
    string Password
);

public record RegisterCustomerAccountDto(
    string FullName,
    string Email,
    string PhoneNumber,
    string Password,
    string Street,
    string City,
    string ZipCode
);

public record RegisterCourierAccountDto(
    string FullName,
    string Email,
    string PhoneNumber,
    int Vehicle,
    string Password
);

public record RegisterAdminAccountDto(
    string FullName,
    string Email,
    string Password,
    string? BootstrapKey
);

public record AuthTokenDto(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    int UserId,
    string Email,
    string DisplayName,
    AppRole Role,
    int? CustomerId,
    int? CourierId
);

public record AdminUserAccountDto(
    int Id,
    string Email,
    string DisplayName,
    AppRole Role,
    bool IsActive,
    int? CustomerId,
    int? CourierId,
    DateTime CreatedAt,
    DateTime? LastLoginAt
);

public record SetAccountStatusDto(
    bool IsActive
);
