using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Application.Interfaces;

public interface IAuthRepository
{
    Task<UserAccount?> GetByEmailAsync(string email);
    Task<UserAccount?> GetByIdAsync(int id);
    Task<List<UserAccount>> GetAllAsync();
    Task<bool> EmailExistsAsync(string email);
    Task<UserAccount> AddAsync(UserAccount account);
    Task UpdateAsync(UserAccount account);
    Task<int> CountByRoleAsync(AppRole role);
}
