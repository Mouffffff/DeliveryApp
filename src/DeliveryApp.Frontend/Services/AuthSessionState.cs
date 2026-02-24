using DeliveryApp.Frontend.Models;

namespace DeliveryApp.Frontend.Services;

public sealed class AuthSessionState
{
    public event Action? Changed;

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken) && ExpiresAt > DateTimeOffset.UtcNow;
    public string? AccessToken { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public int? UserId { get; private set; }
    public string? Email { get; private set; }
    public string? DisplayName { get; private set; }
    public AppRoleCode? Role { get; private set; }
    public int? CustomerId { get; private set; }
    public int? CourierId { get; private set; }

    public bool IsInRole(AppRoleCode role) => IsAuthenticated && Role == role;

    public void SignIn(AuthTokenResponse token)
    {
        AccessToken = token.AccessToken;
        ExpiresAt = token.ExpiresAt;
        UserId = token.UserId;
        Email = token.Email;
        DisplayName = token.DisplayName;
        Role = token.Role;
        CustomerId = token.CustomerId;
        CourierId = token.CourierId;
        Changed?.Invoke();
    }

    public void SignOut()
    {
        AccessToken = null;
        ExpiresAt = default;
        UserId = null;
        Email = null;
        DisplayName = null;
        Role = null;
        CustomerId = null;
        CourierId = null;
        Changed?.Invoke();
    }
}
