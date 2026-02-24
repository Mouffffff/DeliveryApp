using System.Security.Claims;
using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Api.Security;

public static class UserClaimsExtensions
{
    public static AppRole? GetRole(this ClaimsPrincipal user)
    {
        var role = user.FindFirstValue(ClaimTypes.Role) ?? user.FindFirstValue("role");
        if (Enum.TryParse<AppRole>(role, true, out var parsed))
        {
            return parsed;
        }

        return null;
    }

    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var raw = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }

    public static int? GetCustomerId(this ClaimsPrincipal user)
    {
        var raw = user.FindFirstValue("customerId");
        return int.TryParse(raw, out var id) ? id : null;
    }

    public static int? GetCourierId(this ClaimsPrincipal user)
    {
        var raw = user.FindFirstValue("courierId");
        return int.TryParse(raw, out var id) ? id : null;
    }
}
