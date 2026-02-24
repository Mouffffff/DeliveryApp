using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Interfaces;

public interface ICatalogService
{
    Task<IReadOnlyList<StoreDto>> GetStoresAsync();
    Task<ServiceResult<IReadOnlyList<ProductDto>>> GetStoreProductsAsync(int storeId);
    Task<ServiceResult<StoreDto>> CreateStoreAsync(CreateStoreDto dto);
    Task<ServiceResult<StoreDto>> UpdateStoreAsync(int storeId, UpdateStoreDto dto);
    Task<ServiceResult<bool>> DeleteStoreAsync(int storeId);
    Task<ServiceResult<ProductDto>> CreateProductAsync(CreateProductDto dto);
}
