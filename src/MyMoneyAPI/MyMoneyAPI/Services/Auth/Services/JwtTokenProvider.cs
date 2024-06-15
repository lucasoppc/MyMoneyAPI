using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyMoneyAPI.Features.Users.Models;
using MyMoneyAPI.Services.Auth.Configuration;

namespace MyMoneyAPI.Services.Auth.Services;

public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly JwtOptions _jwtOptions;
    
    public JwtTokenProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    
    public string GenerateToken(params Claim[] claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtOptions.ExpiresInMinutes)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public (Claim[] Claims, bool IsExpired) ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            Claim[] claims = null;

            bool isExpired;
            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token,
                                                                 new TokenValidationParameters
                                                                 {
                                                                     ValidateActor = true,
                                                                     ValidateAudience = true,
                                                                     ValidateLifetime = false,
                                                                     ValidateIssuerSigningKey = true,
                                                                     ValidIssuer = _jwtOptions.Issuer,
                                                                     ValidAudience = _jwtOptions.Audience,
                                                                     ClockSkew = TimeSpan.Zero,
                                                                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key))
                                                                 },
                                                                 out SecurityToken securityToken);

                claims = claimsPrincipal.Claims.ToArray();
                isExpired = securityToken.ValidTo < DateTime.UtcNow;
            }
            catch (SecurityTokenKeyWrapException e)
            {
                throw new Exception("Error while unwrapping token", e);
            }
            catch (Exception e) when (e is SecurityTokenExpiredException || e is SecurityTokenInvalidLifetimeException)
            {
                isExpired = true;
            }
            catch (Exception e)
            {
                throw new Exception("Unknown error while validating token", e);
            }
            return (claims, isExpired);
        }
}