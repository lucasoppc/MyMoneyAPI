using System.Security.Claims;
using MediatR;
using MyMoneyAPI.Features.Users.Repositories;
using MyMoneyAPI.Features.Users.Requests;
using MyMoneyAPI.Features.Users.Responses;
using MyMoneyAPI.Services.Auth.Services;

namespace MyMoneyAPI.Features.Users.Handlers;

public class LoginHandler(IUserRepository userRepository, 
    IJwtTokenProvider tokenProvider,
    IHashService hashService,
    IClaimService claimService)
    : IRequestHandler<LoginRequest, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var loginResponse = new LoginResponse();
        
        var user = await userRepository.GetUserByEmailAsync(request.email, cancellationToken);
        if (user is null) return loginResponse;
        
        var passwordMatch = hashService.VerifyHash(request.password, user.passwordHash);
        if (!passwordMatch) return loginResponse;
        
        var userClaims = claimService.CreateUserClaims(user);
        loginResponse.token = tokenProvider.GenerateToken(userClaims);

        return loginResponse;
    }
}