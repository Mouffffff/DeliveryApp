namespace DeliveryApp.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public List<Address> SavedAddresses { get; set; } = new();

    public static Customer Create(string fullName, string email, string phoneNumber)
    {
        var normalizedName = fullName?.Trim() ?? string.Empty;
        var normalizedEmail = email?.Trim() ?? string.Empty;
        var normalizedPhone = phoneNumber?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            throw new InvalidOperationException("Le nom du client est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(normalizedEmail))
        {
            throw new InvalidOperationException("L'email du client est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(normalizedPhone))
        {
            throw new InvalidOperationException("Le numero de telephone du client est obligatoire.");
        }

        return new Customer
        {
            FullName = normalizedName,
            Email = normalizedEmail,
            PhoneNumber = normalizedPhone
        };
    }
}
