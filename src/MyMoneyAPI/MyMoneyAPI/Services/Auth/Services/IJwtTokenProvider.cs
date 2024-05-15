using System.Security.Claims;

namespace MyMoneyAPI.Services.Auth.Services;

public interface IJwtTokenProvider
{
    string GenerateToken(Claim[] claims);
    (Claim[] Claims, bool IsExpired) ValidateToken(string token);
}