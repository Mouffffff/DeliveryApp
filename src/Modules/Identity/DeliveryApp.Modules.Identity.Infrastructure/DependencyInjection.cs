using DeliveryApp.BuildingBlocks.Application.Abstractions;
using DeliveryApp.Modules.Identity.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryApp.Modules.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityServiceStub>();
        return services;
    }
}

internal sealed class IdentityServiceStub : IIdentityService
{
    public Task<Result<AuthResult>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default) =>
        Task.FromResult(Result<AuthResult>.Failure("Identity module not migrated yet."));
}
