using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Application.Interfaces;

public interface ICatalogRepository
{
    Task<List<Product>> GetProductsByIdsAsync(IReadOnlyCollection<int> ids);
    Task<List<Product>> GetProductsByStoreIdAsync(int storeId);
    Task<List<Store>> GetStoresAsync();
    Task<Store> AddStoreAsync(Store store);
    Task<Product> AddProductAsync(Product product);
    Task<bool> StoreExistsAsync(int storeId);
}
