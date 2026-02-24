using DeliveryApp.Application.DTOs;
using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Application.Interfaces;

public interface ITokenProvider
{
    AuthTokenDto CreateToken(UserAccount account);
}
