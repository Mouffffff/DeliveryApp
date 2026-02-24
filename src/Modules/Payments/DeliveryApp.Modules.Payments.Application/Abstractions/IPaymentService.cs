using DeliveryApp.BuildingBlocks.Application.Abstractions;

namespace DeliveryApp.Modules.Payments.Application.Abstractions;

public interface IPaymentService
{
    Task<Result<PaymentDetails>> PayOrderAsync(PayOrderRequest request, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<PaymentDetails>>> GetOrderPaymentsAsync(int orderId, CancellationToken cancellationToken = default);
}

public sealed record PayOrderRequest(int OrderId, decimal Amount, int Method);
public sealed record PaymentDetails(int Id, int OrderId, decimal Amount, string Method, string Status, DateTimeOffset PaidAt);
