namespace DeliveryApp.Application.DTOs;

public record UpdateDeliveryDto(
    string Title,
    string Description,
    int Status
);