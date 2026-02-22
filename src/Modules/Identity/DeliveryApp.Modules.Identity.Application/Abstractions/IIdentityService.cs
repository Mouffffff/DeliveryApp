using DeliveryApp.BuildingBlocks.Application.Abstractions;

namespace DeliveryApp.Modules.Identity.Application.Abstractions;

public interface IIdentityService
{
    Task<Result<AuthResult>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default);
}

public sealed record SignInRequest(string Email, string Password);
public sealed record AuthResult(int UserId, string Token, string Role);
