using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Interfaces;

public interface IPaymentAppService
{
    Task<ServiceResult<PaymentDto>> CreatePaymentAsync(int orderId, CreatePaymentDto dto);
    Task<ServiceResult<IReadOnlyList<PaymentDto>>> GetOrderPaymentsAsync(int orderId);
}
