using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Application.Validation;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Service;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _auth;
    private readonly ICustomerRepository _customers;
    private readonly ICourierRepository _couriers;
    private readonly IPasswordService _passwords;
    private readonly ITokenProvider _tokens;

    public AuthService(
        IAuthRepository auth,
        ICustomerRepository customers,
        ICourierRepository couriers,
        IPasswordService passwords,
        ITokenProvider tokens)
    {
        _auth = auth;
        _customers = customers;
        _couriers = couriers;
        _passwords = passwords;
        _tokens = tokens;
    }

    public async Task<ServiceResult<AuthTokenDto>> LoginAsync(LoginRequestDto dto)
    {
        var validation = AuthValidator.ValidateLogin(dto);
        if (!validation.ok)
        {
            return ServiceResult<AuthTokenDto>.Failure("validation_error", validation.error, 400);
        }

        var account = await _auth.GetByEmailAsync(dto.Email);
        if (account is null || !account.IsActive)
        {
            return ServiceResult<AuthTokenDto>.Failure("invalid_credentials", "Identifiants invalides.", 401);
        }

        if (!_passwords.VerifyPassword(dto.Password, account.PasswordHash))
        {
            return ServiceResult<AuthTokenDto>.Failure("invalid_credentials", "Identifiants invalides.", 401);
        }

        account.MarkLogin();
        await _auth.UpdateAsync(account);

        return ServiceResult<AuthTokenDto>.Success(_tokens.CreateToken(account));
    }

    public async Task<ServiceResult<AuthTokenDto>> RegisterCustomerAsync(RegisterCustomerAccountDto dto)
    {
        var validation = AuthValidator.ValidateRegisterCustomer(dto);
        if (!validation.ok)
        {
            return ServiceResult<AuthTokenDto>.Failure("validation_error", validation.error, 400);
        }

        if (await _auth.EmailExistsAsync(dto.Email))
        {
            return ServiceResult<AuthTokenDto>.Failure("email_already_used", "Cet email est deja utilise.", 409);
        }

        var customer = await _customers.AddCustomerAsync(Customer.Create(dto.FullName, dto.Email, dto.PhoneNumber));
        await _customers.AddAddressAsync(Address.Create(dto.Street, dto.City, dto.ZipCode, customer.Id));

        var passwordHash = _passwords.HashPassword(dto.Password);
        var account = UserAccount.Create(
            dto.Email,
            passwordHash,
            dto.FullName,
            AppRole.Customer,
            customerId: customer.Id);

        var created = await _auth.AddAsync(account);
        created.MarkLogin();
        await _auth.UpdateAsync(created);

        return ServiceResult<AuthTokenDto>.Success(_tokens.CreateToken(created));
    }

    public async Task<ServiceResult<AuthTokenDto>> RegisterCourierAsync(RegisterCourierAccountDto dto)
    {
        var validation = AuthValidator.ValidateRegisterCourier(dto);
        if (!validation.ok)
        {
            return ServiceResult<AuthTokenDto>.Failure("validation_error", validation.error, 400);
        }

        if (await _auth.EmailExistsAsync(dto.Email))
        {
            return ServiceResult<AuthTokenDto>.Failure("email_already_used", "Cet email est deja utilise.", 409);
        }

        if (!Enum.IsDefined(typeof(VehicleType), dto.Vehicle))
        {
            return ServiceResult<AuthTokenDto>.Failure("invalid_vehicle", "Le type de vehicule est invalide.", 400);
        }

        var courier = await _couriers.AddCourierAsync(new Courier
        {
            FullName = dto.FullName.Trim(),
            PhoneNumber = dto.PhoneNumber.Trim(),
            Vehicle = (VehicleType)dto.Vehicle,
            IsAvailable = true
        });

        var passwordHash = _passwords.HashPassword(dto.Password);
        var account = UserAccount.Create(
            dto.Email,
            passwordHash,
            dto.FullName,
            AppRole.Courier,
            courierId: courier.Id);

        var created = await _auth.AddAsync(account);
        created.MarkLogin();
        await _auth.UpdateAsync(created);

        return ServiceResult<AuthTokenDto>.Success(_tokens.CreateToken(created));
    }

    public async Task<ServiceResult<AuthTokenDto>> RegisterAdminAsync(RegisterAdminAccountDto dto)
    {
        var validation = AuthValidator.ValidateRegisterAdmin(dto);
        if (!validation.ok)
        {
            return ServiceResult<AuthTokenDto>.Failure("validation_error", validation.error, 400);
        }

        if (await _auth.EmailExistsAsync(dto.Email))
        {
            return ServiceResult<AuthTokenDto>.Failure("email_already_used", "Cet email est deja utilise.", 409);
        }

        var existingAdmins = await _auth.CountByRoleAsync(AppRole.Admin);
        if (existingAdmins > 0 && string.IsNullOrWhiteSpace(dto.BootstrapKey))
        {
            return ServiceResult<AuthTokenDto>.Failure(
                "admin_registration_forbidden",
                "Un admin existe deja. Fournis une bootstrapKey valide.",
                403);
        }

        var passwordHash = _passwords.HashPassword(dto.Password);
        var account = UserAccount.Create(
            dto.Email,
            passwordHash,
            dto.FullName,
            AppRole.Admin);

        var created = await _auth.AddAsync(account);
        created.MarkLogin();
        await _auth.UpdateAsync(created);

        return ServiceResult<AuthTokenDto>.Success(_tokens.CreateToken(created));
    }

    public async Task<IReadOnlyList<AdminUserAccountDto>> GetAccountsAsync()
    {
        var accounts = await _auth.GetAllAsync();
        return accounts
            .Select(ToAdminUserAccountDto)
            .ToList();
    }

    public async Task<ServiceResult<AdminUserAccountDto>> SetAccountStatusAsync(int accountId, bool isActive, int actorUserId)
    {
        if (accountId <= 0)
        {
            return ServiceResult<AdminUserAccountDto>.Failure("invalid_account_id", "L'ID du compte est invalide.", 400);
        }

        if (actorUserId <= 0)
        {
            return ServiceResult<AdminUserAccountDto>.Failure("invalid_actor", "Session admin invalide.", 403);
        }

        if (accountId == actorUserId && !isActive)
        {
            return ServiceResult<AdminUserAccountDto>.Failure(
                "self_block_forbidden",
                "Un admin ne peut pas bloquer son propre compte.",
                400);
        }

        var account = await _auth.GetByIdAsync(accountId);
        if (account is null)
        {
            return ServiceResult<AdminUserAccountDto>.Failure("account_not_found", "Compte introuvable.", 404);
        }

        if (!isActive && account.Role == AppRole.Admin)
        {
            var accounts = await _auth.GetAllAsync();
            var activeAdmins = accounts.Count(a => a.Role == AppRole.Admin && a.IsActive && a.Id != account.Id);
            if (activeAdmins == 0)
            {
                return ServiceResult<AdminUserAccountDto>.Failure(
                    "last_admin_forbidden",
                    "Impossible de bloquer le dernier compte admin actif.",
                    409);
            }
        }

        account.SetActive(isActive);
        await _auth.UpdateAsync(account);

        return ServiceResult<AdminUserAccountDto>.Success(ToAdminUserAccountDto(account));
    }

    private static AdminUserAccountDto ToAdminUserAccountDto(UserAccount account) =>
        new(
            account.Id,
            account.Email,
            account.DisplayName,
            account.Role,
            account.IsActive,
            account.CustomerId,
            account.CourierId,
            account.CreatedAt,
            account.LastLoginAt);
}
