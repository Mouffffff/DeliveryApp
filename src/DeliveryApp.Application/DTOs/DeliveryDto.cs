using System;

namespace DeliveryApp.Application.DTOs;

public record DeliveryDto(
    int Id,
    string Title,
    string Description,
    int Status,
    DateTimeOffset? CreatedAt,
    DateTimeOffset? CompletedAt
);