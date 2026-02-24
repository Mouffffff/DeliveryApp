using DeliveryApp.BuildingBlocks.Domain.Abstractions;

namespace DeliveryApp.Modules.Catalog.Domain.Entities;

public sealed class Product : Entity
{
    public int StoreId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
}
