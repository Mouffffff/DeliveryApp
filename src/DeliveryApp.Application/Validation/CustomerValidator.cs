using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Validation;

public static class CustomerValidator
{
    public static (bool ok, string error) ValidateCreateCustomer(CreateCustomerDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            return (false, "Le nom du client est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains('@'))
        {
            return (false, "L'email du client est invalide.");
        }

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
        {
            return (false, "Le numero de telephone du client est obligatoire.");
        }

        return (true, string.Empty);
    }

    public static (bool ok, string error) ValidateCreateAddress(CreateAddressDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Street) || string.IsNullOrWhiteSpace(dto.City) || string.IsNullOrWhiteSpace(dto.ZipCode))
        {
            return (false, "Les champs d'adresse Street, City et ZipCode sont obligatoires.");
        }

        if (dto.CustomerId is <= 0)
        {
            return (false, "L'ID client de l'adresse doit etre strictement positif.");
        }

        return (true, string.Empty);
    }
}
