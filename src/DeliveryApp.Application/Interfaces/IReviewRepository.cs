using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Application.Interfaces;

public interface IReviewRepository
{
    Task<Review> AddReviewAsync(Review review);
    Task<List<Review>> GetReviewsByOrderIdAsync(int orderId);
    Task<bool> ReviewExistsForOrderAsync(int orderId);
}
