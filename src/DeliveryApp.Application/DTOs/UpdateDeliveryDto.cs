namespace DeliveryApp.Application.DTOs;

public record UpdateDeliveryDto(
    int Status,
    int? CourierId
);
