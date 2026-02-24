using DeliveryApp.Api.Security;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/login", async (DeliveryApp.Application.DTOs.LoginRequestDto dto, IAuthService auth) =>
        {
            var result = await auth.LoginAsync(dto);
            return ToHttpResult(result);
        });

        group.MapPost("/register/customer", async (DeliveryApp.Application.DTOs.RegisterCustomerAccountDto dto, IAuthService auth) =>
        {
            var result = await auth.RegisterCustomerAsync(dto);
            return ToHttpResult(result);
        });

        group.MapPost("/register/courier", async (DeliveryApp.Application.DTOs.RegisterCourierAccountDto dto, IAuthService auth) =>
        {
            var result = await auth.RegisterCourierAsync(dto);
            return ToHttpResult(result);
        });

        group.MapPost("/register/admin", async (
            DeliveryApp.Application.DTOs.RegisterAdminAccountDto dto,
            IAuthService auth,
            IAuthRepository authRepository,
            IConfiguration configuration) =>
        {
            var existingAdmins = await authRepository.CountByRoleAsync(AppRole.Admin);
            if (existingAdmins > 0)
            {
                var expectedKey = configuration["Auth:AdminBootstrapKey"];
                if (string.IsNullOrWhiteSpace(expectedKey) ||
                    string.IsNullOrWhiteSpace(dto.BootstrapKey) ||
                    !string.Equals(expectedKey, dto.BootstrapKey, StringComparison.Ordinal))
                {
                    return Results.Problem(
                        title: "Request failed",
                        detail: "Bootstrap admin interdite sans cle valide.",
                        statusCode: 403,
                        extensions: new Dictionary<string, object?> { ["errorCode"] = "admin_bootstrap_forbidden" });
                }
            }

            var result = await auth.RegisterAdminAsync(dto);
            return ToHttpResult(result);
        });

        group.MapGet("/me", (HttpContext httpContext) =>
        {
            var user = httpContext.User;
            var role = user.GetRole();

            if (role is null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(new
            {
                userId = user.GetUserId(),
                email = user.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
                displayName = user.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
                role = role.ToString(),
                customerId = user.GetCustomerId(),
                courierId = user.GetCourierId()
            });
        }).RequireAuthorization();

        group.MapGet("/admin/accounts", async (IAuthService auth) =>
        {
            var accounts = await auth.GetAccountsAsync();
            return Results.Ok(accounts);
        }).RequireAuthorization("AdminOnly");

        group.MapPatch("/admin/accounts/{accountId:int}/status", async (
            int accountId,
            SetAccountStatusDto dto,
            HttpContext httpContext,
            IAuthService auth) =>
        {
            var actorUserId = httpContext.User.GetUserId();
            if (!actorUserId.HasValue)
            {
                return Results.Forbid();
            }

            var result = await auth.SetAccountStatusAsync(accountId, dto.IsActive, actorUserId.Value);
            return ToHttpResult(result);
        }).RequireAuthorization("AdminOnly");
    }

    private static IResult ToHttpResult<T>(DeliveryApp.Application.Common.ServiceResult<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        var error = result.Error!;
        return Results.Problem(
            title: "Request failed",
            detail: error.Message,
            statusCode: error.StatusCode,
            extensions: new Dictionary<string, object?>
            {
                ["errorCode"] = error.Code
            });
    }
}
