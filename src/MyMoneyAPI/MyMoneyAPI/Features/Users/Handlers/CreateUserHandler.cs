using System.Security.Claims;
using MediatR;
using MyMoneyAPI.Common.Exceptions;
using MyMoneyAPI.Features.Users.Models;
using MyMoneyAPI.Features.Users.Repositories;
using MyMoneyAPI.Features.Users.Requests;
using MyMoneyAPI.Features.Users.Responses;
using MyMoneyAPI.Services.Auth.Services;

namespace MyMoneyAPI.Features.Users.Handlers;

public class CreateUserHandler(
    IUserRepository userRepository,
    IHashService hashService,
    IJwtTokenProvider tokenProvider,
    IClaimService claimService)
    : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userEntity = new UserEntity
        {
            id = Guid.NewGuid().ToString(),
            name = request.Name,
            email = request.Email,
            passwordHash = hashService.GenerateHash(request.Password),
            defaultCurrency = request.DefaultCurrency
        };

        var existingUser = await userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidUserInputException("User with this email already exists.");
        }
        
        await userRepository.CreateUserAsync(userEntity, cancellationToken);
        
        var userClaims = claimService.CreateUserClaims(userEntity);
        var token = tokenProvider.GenerateToken(userClaims);
        
        return new CreateUserResponse(userEntity.id, 
            userEntity.name, 
            userEntity.email, 
            userEntity.defaultCurrency,
            token);
    }
}