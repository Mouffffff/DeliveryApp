using DeliveryApp.BuildingBlocks.Application.Abstractions;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Modules.Payments.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryApp.Modules.Payments.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentsModule(this IServiceCollection services)
    {
        services.AddScoped<IPaymentService, PaymentServiceAdapter>();
        return services;
    }
}

internal sealed class PaymentServiceAdapter : IPaymentService
{
    private readonly IPaymentAppService _payments;

    public PaymentServiceAdapter(IPaymentAppService payments)
    {
        _payments = payments;
    }

    public async Task<Result<PaymentDetails>> PayOrderAsync(PayOrderRequest request, CancellationToken cancellationToken = default)
    {
        var dto = new CreatePaymentDto(request.Amount, request.Method);
        var result = await _payments.CreatePaymentAsync(request.OrderId, dto);
        if (!result.IsSuccess || result.Value is null)
        {
            return Result<PaymentDetails>.Failure(result.Error?.Message ?? "Payment failed.");
        }

        return Result<PaymentDetails>.Success(ToPaymentDetails(result.Value));
    }

    public async Task<Result<IReadOnlyList<PaymentDetails>>> GetOrderPaymentsAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var result = await _payments.GetOrderPaymentsAsync(orderId);
        if (!result.IsSuccess || result.Value is null)
        {
            return Result<IReadOnlyList<PaymentDetails>>.Failure(result.Error?.Message ?? "Payments not found.");
        }

        return Result<IReadOnlyList<PaymentDetails>>.Success(result.Value.Select(ToPaymentDetails).ToList());
    }

    private static PaymentDetails ToPaymentDetails(PaymentDto payment) =>
        new(payment.Id, payment.OrderId, payment.Amount, payment.Method, payment.Status, payment.PaymentDate);
}
