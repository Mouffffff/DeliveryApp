using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Interfaces;

public interface IReviewService
{
    Task<ServiceResult<ReviewDto>> CreateOrderReviewAsync(int orderId, CreateReviewDto dto);
    Task<ServiceResult<IReadOnlyList<ReviewDto>>> GetOrderReviewsAsync(int orderId);
}
