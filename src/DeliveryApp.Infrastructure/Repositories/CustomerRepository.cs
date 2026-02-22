using DeliveryApp.Application.Interfaces;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Customer>> GetCustomersAsync() =>
        _db.Customers
            .OrderBy(c => c.FullName)
            .ToListAsync();

    public Task<List<Address>> GetAddressesAsync() =>
        _db.Addresses
            .OrderBy(a => a.City)
            .ThenBy(a => a.Street)
            .ToListAsync();

    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
        return customer;
    }

    public async Task<Address> AddAddressAsync(Address address)
    {
        _db.Addresses.Add(address);
        await _db.SaveChangesAsync();
        return address;
    }

    public Task<bool> CustomerExistsAsync(int customerId) =>
        _db.Customers.AnyAsync(c => c.Id == customerId);

    public Task<bool> AddressExistsAsync(int addressId) =>
        _db.Addresses.AnyAsync(a => a.Id == addressId);
}
