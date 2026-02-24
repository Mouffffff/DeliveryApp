using DeliveryApp.BuildingBlocks.Application.Abstractions;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Modules.Dispatch.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryApp.Modules.Dispatch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDispatchModule(this IServiceCollection services)
    {
        services.AddScoped<IDispatchService, DispatchServiceAdapter>();
        return services;
    }
}

internal sealed class DispatchServiceAdapter : IDispatchService
{
    private readonly ICourierService _couriers;

    public DispatchServiceAdapter(ICourierService couriers)
    {
        _couriers = couriers;
    }

    public async Task<Result<AssignmentDetails>> AssignCourierAsync(int orderId, int courierId, CancellationToken cancellationToken = default)
    {
        var result = await _couriers.AssignCourierAsync(orderId, courierId);
        if (!result.IsSuccess || result.Value is null)
        {
            return Result<AssignmentDetails>.Failure(result.Error?.Message ?? "Assignment failed.");
        }

        return Result<AssignmentDetails>.Success(
            new AssignmentDetails(result.Value.Id, courierId, DateTimeOffset.UtcNow));
    }

    public async Task<Result<IReadOnlyList<CourierSummary>>> GetAvailableCouriersAsync(CancellationToken cancellationToken = default)
    {
        var couriers = await _couriers.GetAvailableCouriersAsync();
        return Result<IReadOnlyList<CourierSummary>>.Success(
            couriers.Select(c => new CourierSummary(c.Id, c.FullName, c.Vehicle)).ToList());
    }
}
