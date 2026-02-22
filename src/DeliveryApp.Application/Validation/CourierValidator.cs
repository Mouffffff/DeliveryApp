using DeliveryApp.Application.DTOs;
using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Application.Validation;

public static class CourierValidator
{
    public static (bool ok, string error) ValidateCreate(CreateCourierDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            return (false, "Le nom du coursier est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
        {
            return (false, "Le numero de telephone du coursier est obligatoire.");
        }

        if (!Enum.IsDefined(typeof(VehicleType), dto.Vehicle))
        {
            return (false, "Le type de vehicule est invalide.");
        }

        return (true, string.Empty);
    }
}
