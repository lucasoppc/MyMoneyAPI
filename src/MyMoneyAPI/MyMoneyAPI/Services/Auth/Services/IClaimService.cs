using System.Security.Claims;
using MyMoneyAPI.Features.Users.Models;

namespace MyMoneyAPI.Services.Auth.Services;

public interface IClaimService
{
    Claim[] CreateUserClaims(UserEntity user);
}