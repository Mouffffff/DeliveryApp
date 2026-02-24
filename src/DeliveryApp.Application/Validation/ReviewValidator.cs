using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Validation;

public static class ReviewValidator
{
    public static (bool ok, string error) ValidateCreate(CreateReviewDto dto)
    {
        if (dto.Rating < 1 || dto.Rating > 5)
        {
            return (false, "La note doit etre comprise entre 1 et 5.");
        }

        if (string.IsNullOrWhiteSpace(dto.Comment))
        {
            return (false, "Le commentaire est obligatoire.");
        }

        return (true, string.Empty);
    }
}
