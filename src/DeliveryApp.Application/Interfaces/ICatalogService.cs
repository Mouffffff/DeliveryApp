using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Interfaces;

public interface ICatalogService
{
    Task<IReadOnlyList<StoreDto>> GetStoresAsync();
    Task<ServiceResult<IReadOnlyList<ProductDto>>> GetStoreProductsAsync(int storeId);
    Task<ServiceResult<StoreDto>> CreateStoreAsync(CreateStoreDto dto);
    Task<ServiceResult<ProductDto>> CreateProductAsync(CreateProductDto dto);
}
