using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Application.Validation;
using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Service;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customers;

    public CustomerService(ICustomerRepository customers)
    {
        _customers = customers;
    }

    public async Task<IReadOnlyList<CustomerDto>> GetCustomersAsync()
    {
        var customers = await _customers.GetCustomersAsync();
        return customers.Select(c => new CustomerDto(c.Id, c.FullName, c.Email, c.PhoneNumber)).ToList();
    }

    public async Task<IReadOnlyList<AddressDto>> GetAddressesAsync()
    {
        var addresses = await _customers.GetAddressesAsync();
        return addresses.Select(a => new AddressDto(a.Id, a.Street, a.City, a.ZipCode, a.CustomerId)).ToList();
    }

    public async Task<ServiceResult<CustomerDto>> CreateCustomerAsync(CreateCustomerDto dto)
    {
        var validation = CustomerValidator.ValidateCreateCustomer(dto);
        if (!validation.ok)
        {
            return ServiceResult<CustomerDto>.Failure("validation_error", validation.error, 400);
        }

        var created = await _customers.AddCustomerAsync(Customer.Create(dto.FullName, dto.Email, dto.PhoneNumber));

        return ServiceResult<CustomerDto>.Success(new CustomerDto(
            created.Id,
            created.FullName,
            created.Email,
            created.PhoneNumber));
    }

    public async Task<ServiceResult<AddressDto>> CreateAddressAsync(CreateAddressDto dto)
    {
        var validation = CustomerValidator.ValidateCreateAddress(dto);
        if (!validation.ok)
        {
            return ServiceResult<AddressDto>.Failure("validation_error", validation.error, 400);
        }

        if (dto.CustomerId.HasValue && !await _customers.CustomerExistsAsync(dto.CustomerId.Value))
        {
            return ServiceResult<AddressDto>.Failure("customer_not_found", "Le client associe a l'adresse est introuvable.", 404);
        }

        var created = await _customers.AddAddressAsync(Address.Create(dto.Street, dto.City, dto.ZipCode, dto.CustomerId));

        return ServiceResult<AddressDto>.Success(new AddressDto(
            created.Id,
            created.Street,
            created.City,
            created.ZipCode,
            created.CustomerId));
    }
}
