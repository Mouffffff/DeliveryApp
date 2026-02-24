using DeliveryApp.Application.DTOs;
using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Application.Validation;

public static class PaymentValidator
{
    public static (bool ok, string error) ValidateCreate(CreatePaymentDto dto)
    {
        if (!Enum.IsDefined(typeof(PaymentMethod), dto.Method))
        {
            return (false, "La methode de paiement est invalide.");
        }

        return (true, string.Empty);
    }
}
