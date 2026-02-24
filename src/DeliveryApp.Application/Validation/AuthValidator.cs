using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Validation;

public static class AuthValidator
{
    public static (bool ok, string error) ValidateLogin(LoginRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return (false, "L'email est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            return (false, "Le mot de passe est obligatoire.");
        }

        return (true, string.Empty);
    }

    public static (bool ok, string error) ValidateRegisterCustomer(RegisterCustomerAccountDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            return (false, "Le nom complet est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return (false, "L'email est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
        {
            return (false, "Le numero de telephone est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 8)
        {
            return (false, "Le mot de passe doit contenir au moins 8 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(dto.Street) ||
            string.IsNullOrWhiteSpace(dto.City) ||
            string.IsNullOrWhiteSpace(dto.ZipCode))
        {
            return (false, "L'adresse complete est obligatoire.");
        }

        return (true, string.Empty);
    }

    public static (bool ok, string error) ValidateRegisterCourier(RegisterCourierAccountDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            return (false, "Le nom complet est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return (false, "L'email est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
        {
            return (false, "Le numero de telephone est obligatoire.");
        }

        if (dto.Vehicle < 0)
        {
            return (false, "Le type de vehicule est invalide.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 8)
        {
            return (false, "Le mot de passe doit contenir au moins 8 caracteres.");
        }

        return (true, string.Empty);
    }

    public static (bool ok, string error) ValidateRegisterAdmin(RegisterAdminAccountDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            return (false, "Le nom complet est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return (false, "L'email est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 10)
        {
            return (false, "Le mot de passe admin doit contenir au moins 10 caracteres.");
        }

        return (true, string.Empty);
    }
}
