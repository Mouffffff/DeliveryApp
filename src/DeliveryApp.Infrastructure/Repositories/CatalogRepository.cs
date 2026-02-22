using DeliveryApp.Application.Interfaces;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Repositories;

public class CatalogRepository : ICatalogRepository
{
    private readonly AppDbContext _db;

    public CatalogRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Product>> GetProductsByIdsAsync(IReadOnlyCollection<int> ids) =>
        _db.Products
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();

    public Task<List<Product>> GetProductsByStoreIdAsync(int storeId) =>
        _db.Products
            .Where(p => p.StoreId == storeId)
            .OrderBy(p => p.Name)
            .ToListAsync();

    public Task<List<Store>> GetStoresAsync() =>
        _db.Stores
            .Include(s => s.Location)
            .OrderBy(s => s.Name)
            .ToListAsync();

    public async Task<Store> AddStoreAsync(Store store)
    {
        _db.Stores.Add(store);
        await _db.SaveChangesAsync();
        return await _db.Stores.Include(s => s.Location).FirstAsync(s => s.Id == store.Id);
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product;
    }

    public Task<bool> StoreExistsAsync(int storeId) =>
        _db.Stores.AnyAsync(s => s.Id == storeId);
}
