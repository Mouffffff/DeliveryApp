namespace DeliveryApp.Api.Security;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "DeliveryApp.Api";
    public string Audience { get; set; } = "DeliveryApp.Clients";
    public string Secret { get; set; } = "change-this-secret-key-with-at-least-32-chars";
    public int AccessTokenMinutes { get; set; } = 180;
}
