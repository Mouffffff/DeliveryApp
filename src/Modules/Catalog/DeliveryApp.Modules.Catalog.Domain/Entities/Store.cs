using DeliveryApp.BuildingBlocks.Domain.Abstractions;

namespace DeliveryApp.Modules.Catalog.Domain.Entities;

public sealed class Store : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;

    private readonly List<Product> _products = [];
    public IReadOnlyCollection<Product> Products => _products;
}
