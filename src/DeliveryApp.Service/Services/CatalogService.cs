using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Application.Validation;
using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Service;

public class CatalogService : ICatalogService
{
    private readonly ICatalogRepository _catalog;
    private readonly ICustomerRepository _customers;

    public CatalogService(ICatalogRepository catalog, ICustomerRepository customers)
    {
        _catalog = catalog;
        _customers = customers;
    }

    public async Task<IReadOnlyList<StoreDto>> GetStoresAsync()
    {
        var stores = await _catalog.GetStoresAsync();
        return stores
            .Select(s => new StoreDto(
                s.Id,
                s.Name,
                s.Category,
                s.Location.Street,
                s.Location.City,
                s.Location.ZipCode))
            .ToList();
    }

    public async Task<ServiceResult<IReadOnlyList<ProductDto>>> GetStoreProductsAsync(int storeId)
    {
        if (storeId <= 0)
        {
            return ServiceResult<IReadOnlyList<ProductDto>>.Failure("invalid_store_id", "L'ID du magasin doit etre superieur a 0.", 400);
        }

        if (!await _catalog.StoreExistsAsync(storeId))
        {
            return ServiceResult<IReadOnlyList<ProductDto>>.Failure("store_not_found", "Le magasin n'existe pas.", 404);
        }

        var products = await _catalog.GetProductsByStoreIdAsync(storeId);
        return ServiceResult<IReadOnlyList<ProductDto>>.Success(
            products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.StoreId)).ToList());
    }

    public async Task<ServiceResult<StoreDto>> CreateStoreAsync(CreateStoreDto dto)
    {
        var validation = CatalogValidator.ValidateCreateStore(dto);
        if (!validation.ok)
        {
            return ServiceResult<StoreDto>.Failure("validation_error", validation.error, 400);
        }

        if (!await _customers.AddressExistsAsync(dto.AddressId))
        {
            return ServiceResult<StoreDto>.Failure("address_not_found", "L'adresse du magasin est introuvable.", 404);
        }

        var created = await _catalog.AddStoreAsync(Store.Create(dto.Name, dto.Category, dto.AddressId));

        return ServiceResult<StoreDto>.Success(new StoreDto(
            created.Id,
            created.Name,
            created.Category,
            created.Location.Street,
            created.Location.City,
            created.Location.ZipCode));
    }

    public async Task<ServiceResult<ProductDto>> CreateProductAsync(CreateProductDto dto)
    {
        var validation = CatalogValidator.ValidateCreateProduct(dto);
        if (!validation.ok)
        {
            return ServiceResult<ProductDto>.Failure("validation_error", validation.error, 400);
        }

        if (!await _catalog.StoreExistsAsync(dto.StoreId))
        {
            return ServiceResult<ProductDto>.Failure("store_not_found", "Le magasin du produit est introuvable.", 404);
        }

        var created = await _catalog.AddProductAsync(Product.Create(dto.Name, dto.Price, dto.StoreId));

        return ServiceResult<ProductDto>.Success(new ProductDto(
            created.Id,
            created.Name,
            created.Price,
            created.StoreId));
    }
}
