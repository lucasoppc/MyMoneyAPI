using System.Security.Claims;

namespace MyMoneyAPI.Services.Auth.Services;

public interface IJwtTokenProvider
{
    string GenerateToken(params Claim[] claims);
    (Claim[] Claims, bool IsExpired) ValidateToken(string token);
}