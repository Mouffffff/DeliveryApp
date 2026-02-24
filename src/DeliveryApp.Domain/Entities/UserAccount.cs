using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Domain.Entities;

public class UserAccount
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public AppRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public int? CustomerId { get; set; }
    public int? CourierId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    public static UserAccount Create(
        string email,
        string passwordHash,
        string displayName,
        AppRole role,
        int? customerId = null,
        int? courierId = null)
    {
        var normalizedEmail = email?.Trim().ToLowerInvariant() ?? string.Empty;
        var normalizedHash = passwordHash?.Trim() ?? string.Empty;
        var normalizedDisplayName = displayName?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(normalizedEmail))
        {
            throw new InvalidOperationException("L'email est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(normalizedHash))
        {
            throw new InvalidOperationException("Le mot de passe est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(normalizedDisplayName))
        {
            throw new InvalidOperationException("Le nom d'affichage est obligatoire.");
        }

        return new UserAccount
        {
            Email = normalizedEmail,
            PasswordHash = normalizedHash,
            DisplayName = normalizedDisplayName,
            Role = role,
            CustomerId = customerId,
            CourierId = courierId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
