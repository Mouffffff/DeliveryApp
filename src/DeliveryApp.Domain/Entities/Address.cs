namespace DeliveryApp.Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public int? CustomerId { get; set; }

    public static Address Create(string street, string city, string zipCode, int? customerId)
    {
        var normalizedStreet = street?.Trim() ?? string.Empty;
        var normalizedCity = city?.Trim() ?? string.Empty;
        var normalizedZip = zipCode?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(normalizedStreet) || string.IsNullOrWhiteSpace(normalizedCity) || string.IsNullOrWhiteSpace(normalizedZip))
        {
            throw new InvalidOperationException("Street, City et ZipCode sont obligatoires.");
        }

        if (customerId.HasValue && customerId <= 0)
        {
            throw new InvalidOperationException("L'ID client est invalide.");
        }

        return new Address
        {
            Street = normalizedStreet,
            City = normalizedCity,
            ZipCode = normalizedZip,
            CustomerId = customerId
        };
    }
}
