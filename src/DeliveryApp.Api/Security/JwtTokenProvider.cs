using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryApp.Api.Security;

public sealed class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _options;

    public JwtTokenProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public AuthTokenDto CreateToken(UserAccount account)
    {
        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.AddMinutes(_options.AccessTokenMinutes <= 0 ? 180 : _options.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, account.Email),
            new(JwtRegisteredClaimNames.UniqueName, account.DisplayName),
            new(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new(ClaimTypes.Name, account.DisplayName),
            new(ClaimTypes.Email, account.Email),
            new(ClaimTypes.Role, account.Role.ToString()),
            new("role", account.Role.ToString())
        };

        if (account.CustomerId.HasValue)
        {
            claims.Add(new Claim("customerId", account.CustomerId.Value.ToString()));
        }

        if (account.CourierId.HasValue)
        {
            claims.Add(new Claim("courierId", account.CourierId.Value.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: creds);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return new AuthTokenDto(
            token,
            expiresAt,
            account.Id,
            account.Email,
            account.DisplayName,
            account.Role,
            account.CustomerId,
            account.CourierId);
    }
}
