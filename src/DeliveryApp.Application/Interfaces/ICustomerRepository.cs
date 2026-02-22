using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Application.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetCustomersAsync();
    Task<List<Address>> GetAddressesAsync();
    Task<Customer> AddCustomerAsync(Customer customer);
    Task<Address> AddAddressAsync(Address address);
    Task<bool> CustomerExistsAsync(int customerId);
    Task<bool> AddressExistsAsync(int addressId);
}
