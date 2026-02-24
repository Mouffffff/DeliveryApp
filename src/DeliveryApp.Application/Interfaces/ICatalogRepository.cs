using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Application.Interfaces;

public interface ICatalogRepository
{
    Task<List<Product>> GetProductsByIdsAsync(IReadOnlyCollection<int> ids);
    Task<List<Product>> GetProductsByStoreIdAsync(int storeId);
    Task<List<Store>> GetStoresAsync();
    Task<Store?> GetStoreByIdAsync(int storeId);
    Task<Store> AddStoreAsync(Store store);
    Task<Store> UpdateStoreAsync(Store store);
    Task DeleteStoreAsync(Store store);
    Task<Product> AddProductAsync(Product product);
    Task<bool> StoreExistsAsync(int storeId);
    Task<bool> StoreHasOrdersAsync(int storeId);
}
