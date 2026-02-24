using DeliveryApp.Application.Interfaces;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Enums;
using DeliveryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _db;

    public AuthRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<UserAccount?> GetByEmailAsync(string email)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return _db.UserAccounts.FirstOrDefaultAsync(u => u.Email == normalized);
    }

    public Task<UserAccount?> GetByIdAsync(int id) =>
        _db.UserAccounts.FirstOrDefaultAsync(u => u.Id == id);

    public Task<List<UserAccount>> GetAllAsync() =>
        _db.UserAccounts
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();

    public Task<bool> EmailExistsAsync(string email)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return _db.UserAccounts.AnyAsync(u => u.Email == normalized);
    }

    public async Task<UserAccount> AddAsync(UserAccount account)
    {
        _db.UserAccounts.Add(account);
        await _db.SaveChangesAsync();
        return account;
    }

    public async Task UpdateAsync(UserAccount account)
    {
        _db.UserAccounts.Update(account);
        await _db.SaveChangesAsync();
    }

    public Task<int> CountByRoleAsync(AppRole role) =>
        _db.UserAccounts.CountAsync(u => u.Role == role);
}
