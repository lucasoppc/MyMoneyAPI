using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyMoneyAPI.Services.Auth.Configuration;
using MyMoneyAPI.Services.Auth.Services;

namespace MyMoneyAPI.Services.Auth;

public static class JwtConfigurationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwtConfigOptions = config.GetSection("jwt").Get<JwtOptions>();
        
        services.Configure<JwtOptions>(config.GetSection("jwt"));
        services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();
        services.AddSingleton<IHashService, HashService>();
        services.AddScoped<IClaimService, ClaimService>();
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfigOptions.Issuer,
                ValidAudience = jwtConfigOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigOptions.Key))
            };
        });

        return services;
    }
}