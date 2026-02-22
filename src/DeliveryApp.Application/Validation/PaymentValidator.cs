using DeliveryApp.Application.DTOs;
using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Application.Validation;

public static class PaymentValidator
{
    public static (bool ok, string error) ValidateCreate(CreatePaymentDto dto)
    {
        if (dto.Amount <= 0)
        {
            return (false, "Le montant du paiement doit etre superieur a 0.");
        }

        if (!Enum.IsDefined(typeof(PaymentMethod), dto.Method))
        {
            return (false, "La methode de paiement est invalide.");
        }

        return (true, string.Empty);
    }
}
