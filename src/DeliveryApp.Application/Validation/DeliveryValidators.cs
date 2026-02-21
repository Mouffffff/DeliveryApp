using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Validation;

public static class DeliveryValidators
{
    public static (bool ok, string error) Validate(CreateDeliveryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title) || dto.Title.Length > 100)
        {
            return (false, "Title is required and must be <= 100 chars.");
        }

        if (string.IsNullOrWhiteSpace(dto.Description) || dto.Description.Length > 500)
        {
            return (false, "Description is required and must be <= 500 chars.");
        }

        return (true, "");
    }

    public static (bool ok, string error) Validate(UpdateDeliveryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title) || dto.Title.Length > 100)
        {
            return (false, "Title is required and must be <= 100 chars.");
        }

        if (string.IsNullOrWhiteSpace(dto.Description) || dto.Description.Length > 500)
        {
            return (false, "Description is required and must be <= 500 chars.");
        }

        if (dto.Status < 0 || dto.Status > 3)
        {
            return (false, "Status must be between 0 and 3.");
        }

        return (true, "");
    }
}