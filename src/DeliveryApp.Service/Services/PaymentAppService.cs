using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Application.Validation;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Service;

public class PaymentAppService : IPaymentAppService
{
    private readonly IOrderRepository _orders;
    private readonly IPaymentRepository _payments;

    public PaymentAppService(IOrderRepository orders, IPaymentRepository payments)
    {
        _orders = orders;
        _payments = payments;
    }

    public async Task<ServiceResult<PaymentDto>> CreatePaymentAsync(int orderId, CreatePaymentDto dto)
    {
        if (orderId <= 0)
        {
            return ServiceResult<PaymentDto>.Failure("invalid_order_id", "L'ID de commande doit etre superieur a 0.", 400);
        }

        var validation = PaymentValidator.ValidateCreate(dto);
        if (!validation.ok)
        {
            return ServiceResult<PaymentDto>.Failure("validation_error", validation.error, 400);
        }

        var order = await _orders.GetByIdAsync(orderId);
        if (order is null)
        {
            return ServiceResult<PaymentDto>.Failure("order_not_found", "Commande introuvable.", 404);
        }

        if (dto.Amount != order.TotalPrice)
        {
            return ServiceResult<PaymentDto>.Failure("invalid_amount", $"Le montant doit etre egal au total de la commande ({order.TotalPrice}).", 409);
        }

        var existingPayments = await _payments.GetPaymentsByOrderIdAsync(orderId);
        if (existingPayments.Any(p => p.Status == PaymentStatus.Completed))
        {
            return ServiceResult<PaymentDto>.Failure("payment_already_completed", "La commande est deja payee.", 409);
        }

        var created = await _payments.AddPaymentAsync(
            Payment.CreateCompleted(orderId, dto.Amount, (PaymentMethod)dto.Method, DateTime.UtcNow));

        return ServiceResult<PaymentDto>.Success(new PaymentDto(
            created.Id,
            created.OrderId,
            created.Amount,
            created.Method.ToString(),
            created.Status.ToString(),
            new DateTimeOffset(DateTime.SpecifyKind(created.PaymentDate, DateTimeKind.Utc))));
    }

    public async Task<ServiceResult<IReadOnlyList<PaymentDto>>> GetOrderPaymentsAsync(int orderId)
    {
        if (orderId <= 0)
        {
            return ServiceResult<IReadOnlyList<PaymentDto>>.Failure("invalid_order_id", "L'ID de commande doit etre superieur a 0.", 400);
        }

        var order = await _orders.GetByIdAsync(orderId);
        if (order is null)
        {
            return ServiceResult<IReadOnlyList<PaymentDto>>.Failure("order_not_found", "Commande introuvable.", 404);
        }

        var payments = await _payments.GetPaymentsByOrderIdAsync(orderId);
        return ServiceResult<IReadOnlyList<PaymentDto>>.Success(
            payments.Select(p => new PaymentDto(
                p.Id,
                p.OrderId,
                p.Amount,
                p.Method.ToString(),
                p.Status.ToString(),
                new DateTimeOffset(DateTime.SpecifyKind(p.PaymentDate, DateTimeKind.Utc))))
            .ToList());
    }
}
