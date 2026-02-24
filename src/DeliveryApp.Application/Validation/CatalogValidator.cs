using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Validation;

public static class CatalogValidator
{
    public static (bool ok, string error) ValidateCreateStore(CreateStoreDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return (false, "Le nom du magasin est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(dto.Category))
        {
            return (false, "La categorie du magasin est obligatoire.");
        }

        if (dto.AddressId <= 0)
        {
            return (false, "L'ID de l'adresse du magasin est invalide.");
        }

        return (true, string.Empty);
    }

    public static (bool ok, string error) ValidateCreateProduct(CreateProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return (false, "Le nom du produit est obligatoire.");
        }

        if (dto.Price <= 0)
        {
            return (false, "Le prix du produit doit etre superieur a 0.");
        }

        if (dto.StoreId <= 0)
        {
            return (false, "L'ID du magasin est invalide.");
        }

        return (true, string.Empty);
    }

    public static (bool ok, string error) ValidateUpdateStore(UpdateStoreDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return (false, "Le nom du magasin est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(dto.Category))
        {
            return (false, "La categorie du magasin est obligatoire.");
        }

        if (dto.AddressId <= 0)
        {
            return (false, "L'ID de l'adresse du magasin est invalide.");
        }

        return (true, string.Empty);
    }
}
