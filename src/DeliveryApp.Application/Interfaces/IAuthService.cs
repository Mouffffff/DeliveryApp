using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;

namespace DeliveryApp.Application.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<AuthTokenDto>> LoginAsync(LoginRequestDto dto);
    Task<ServiceResult<AuthTokenDto>> RegisterCustomerAsync(RegisterCustomerAccountDto dto);
    Task<ServiceResult<AuthTokenDto>> RegisterCourierAsync(RegisterCourierAccountDto dto);
    Task<ServiceResult<AuthTokenDto>> RegisterAdminAsync(RegisterAdminAccountDto dto);
    Task<IReadOnlyList<AdminUserAccountDto>> GetAccountsAsync();
    Task<ServiceResult<AdminUserAccountDto>> SetAccountStatusAsync(int accountId, bool isActive, int actorUserId);
}
