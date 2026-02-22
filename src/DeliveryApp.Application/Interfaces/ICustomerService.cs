using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Interfaces;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerDto>> GetCustomersAsync();
    Task<IReadOnlyList<AddressDto>> GetAddressesAsync();
    Task<ServiceResult<CustomerDto>> CreateCustomerAsync(CreateCustomerDto dto);
    Task<ServiceResult<AddressDto>> CreateAddressAsync(CreateAddressDto dto);
}
