namespace DeliveryApp.Modules.Catalog.Application.Abstractions;

public interface ICatalogQueries
{
    Task<IReadOnlyList<StoreSummary>> GetStoresAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductSummary>> GetProductsAsync(int storeId, CancellationToken cancellationToken = default);
}

public sealed record StoreSummary(int Id, string Name, string Category);
public sealed record ProductSummary(int Id, string Name, decimal Price, int StoreId);
