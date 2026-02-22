using DeliveryApp.Application.DTOs;
using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Application.Validation;

public static class OrderValidator
{
    public static (bool ok, string error) ValidateCreate(CreateDeliveryDto dto)
    {
        if (dto.CustomerId <= 0)
        {
            return (false, "L'ID du client est requis.");
        }

        if (dto.StoreId <= 0)
        {
            return (false, "L'ID du magasin est requis.");
        }

        if (dto.DeliveryAddressId <= 0)
        {
            return (false, "L'ID de l'adresse de livraison est requis.");
        }

        var hasLegacyProducts = dto.ProductIds is { Count: > 0 };
        var hasItems = dto.Items is { Count: > 0 };
        if (!hasLegacyProducts && !hasItems)
        {
            return (false, "La commande doit contenir au moins un produit.");
        }

        if (hasLegacyProducts && dto.ProductIds.Any(id => id <= 0))
        {
            return (false, "Tous les IDs produits doivent etre valides.");
        }

        if (hasItems && dto.Items.Any(i => i.ProductId <= 0 || i.Quantity <= 0))
        {
            return (false, "Chaque ligne de commande doit avoir un produit valide et une quantite superieure a 0.");
        }

        return (true, string.Empty);
    }

    public static (bool ok, string error) ValidateUpdate(UpdateDeliveryDto dto)
    {
        if (!Enum.IsDefined(typeof(OrderStatus), dto.Status))
        {
            return (false, "Le statut fourni est invalide.");
        }

        if (dto.CourierId is <= 0)
        {
            return (false, "L'ID du coursier doit etre strictement positif.");
        }

        return (true, string.Empty);
    }
}
