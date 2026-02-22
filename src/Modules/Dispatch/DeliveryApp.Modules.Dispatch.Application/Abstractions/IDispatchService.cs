using DeliveryApp.BuildingBlocks.Application.Abstractions;

namespace DeliveryApp.Modules.Dispatch.Application.Abstractions;

public interface IDispatchService
{
    Task<Result<AssignmentDetails>> AssignCourierAsync(int orderId, int courierId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<CourierSummary>>> GetAvailableCouriersAsync(CancellationToken cancellationToken = default);
}

public sealed record CourierSummary(int Id, string FullName, string Vehicle);
public sealed record AssignmentDetails(int OrderId, int CourierId, DateTimeOffset AssignedAt);
