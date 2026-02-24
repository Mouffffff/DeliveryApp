using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Application.Validation;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Enums;
using DeliveryApp.Service.Services.Helpers;

namespace DeliveryApp.Service;

public class ReviewService : IReviewService
{
    private readonly IOrderRepository _orders;
    private readonly IReviewRepository _reviews;

    public ReviewService(IOrderRepository orders, IReviewRepository reviews)
    {
        _orders = orders;
        _reviews = reviews;
    }

    public async Task<ServiceResult<ReviewDto>> CreateOrderReviewAsync(int orderId, CreateReviewDto dto)
    {
        if (orderId <= 0)
        {
            return ServiceResult<ReviewDto>.Failure("invalid_order_id", "L'ID de commande doit etre superieur a 0.", 400);
        }

        var validation = ReviewValidator.ValidateCreate(dto);
        if (!validation.ok)
        {
            return ServiceResult<ReviewDto>.Failure("validation_error", validation.error, 400);
        }

        var order = await _orders.GetByIdAsync(orderId);
        if (order is null)
        {
            return ServiceResult<ReviewDto>.Failure("order_not_found", "Commande introuvable.", 404);
        }

        if (order.Status != OrderStatus.Delivered)
        {
            return ServiceResult<ReviewDto>.Failure("order_not_delivered", "Un avis ne peut etre cree qu'apres livraison.", 409);
        }

        if (await _reviews.ReviewExistsForOrderAsync(orderId))
        {
            return ServiceResult<ReviewDto>.Failure("review_already_exists", "Un avis existe deja pour cette commande.", 409);
        }

        var created = await _reviews.AddReviewAsync(Review.Create(orderId, dto.Rating, dto.Comment, DateTime.UtcNow));

        return ServiceResult<ReviewDto>.Success(DeliveryDtoMapper.ToReviewDto(created));
    }

    public async Task<ServiceResult<IReadOnlyList<ReviewDto>>> GetOrderReviewsAsync(int orderId)
    {
        if (orderId <= 0)
        {
            return ServiceResult<IReadOnlyList<ReviewDto>>.Failure("invalid_order_id", "L'ID de commande doit etre superieur a 0.", 400);
        }

        if (await _orders.GetByIdAsync(orderId) is null)
        {
            return ServiceResult<IReadOnlyList<ReviewDto>>.Failure("order_not_found", "Commande introuvable.", 404);
        }

        var reviews = await _reviews.GetReviewsByOrderIdAsync(orderId);
        return ServiceResult<IReadOnlyList<ReviewDto>>.Success(reviews.Select(DeliveryDtoMapper.ToReviewDto).ToList());
    }
}
