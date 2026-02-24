using DeliveryApp.Application.Interfaces;
using DeliveryApp.Modules.Catalog.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryApp.Modules.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services)
    {
        services.AddScoped<ICatalogQueries, CatalogQueriesAdapter>();
        return services;
    }
}

internal sealed class CatalogQueriesAdapter : ICatalogQueries
{
    private readonly ICatalogService _catalogService;

    public CatalogQueriesAdapter(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public async Task<IReadOnlyList<StoreSummary>> GetStoresAsync(CancellationToken cancellationToken = default)
    {
        var stores = await _catalogService.GetStoresAsync();
        return stores.Select(s => new StoreSummary(s.Id, s.Name, s.Category)).ToList();
    }

    public async Task<IReadOnlyList<ProductSummary>> GetProductsAsync(int storeId, CancellationToken cancellationToken = default)
    {
        var result = await _catalogService.GetStoreProductsAsync(storeId);
        if (!result.IsSuccess || result.Value is null)
        {
            return [];
        }

        return result.Value
            .Select(p => new ProductSummary(p.Id, p.Name, p.Price, p.StoreId))
            .ToList();
    }
}
