namespace DeliveryApp.Domain.Entities;

public class Review
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public static Review Create(int orderId, int rating, string comment, DateTime createdAtUtc)
    {
        if (orderId <= 0)
        {
            throw new InvalidOperationException("L'ID de commande doit etre strictement positif.");
        }

        if (rating is < 1 or > 5)
        {
            throw new InvalidOperationException("La note doit etre comprise entre 1 et 5.");
        }

        var normalizedComment = comment?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedComment))
        {
            throw new InvalidOperationException("Le commentaire est obligatoire.");
        }

        return new Review
        {
            OrderId = orderId,
            Rating = rating,
            Comment = normalizedComment,
            CreatedAt = createdAtUtc
        };
    }
}
