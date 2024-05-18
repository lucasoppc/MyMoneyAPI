using System.Security.Claims;
using MyMoneyAPI.Features.Users.Models;

namespace MyMoneyAPI.Services.Auth.Services;

public class ClaimService : IClaimService
{
    public Claim[] CreateUserClaims(UserEntity user)
    {
        return
        [
            new Claim("UserId", user.id),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Name, user.name),
            new Claim("DefaultCurrency", user.defaultCurrency)
        ];
    }
}