using System.Security.Claims;
using MediatR;
using MyMoneyAPI.Features.Users.Models;
using MyMoneyAPI.Features.Users.Repositories;
using MyMoneyAPI.Features.Users.Requests;
using MyMoneyAPI.Features.Users.Responses;
using MyMoneyAPI.Services.Auth.Services;

namespace MyMoneyAPI.Features.Users.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;
    private readonly IJwtTokenProvider _tokenProvider;

    public CreateUserHandler(IUserRepository userRepository, IHashService hashService, IJwtTokenProvider tokenProvider)
    {
        _userRepository = userRepository;
        _hashService = hashService;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userEntity = new UserEntity
        {
            id = Guid.NewGuid().ToString(),
            name = request.Name,
            email = request.Email,
            passwordHash = _hashService.GenerateHash(request.Password),
            defaultCurrency = request.DefaultCurrency
        };

        userEntity = await _userRepository.CreateUserAsync(userEntity);

        var emailClaim = new Claim(ClaimTypes.Email, userEntity.email);
        var idClaim = new Claim(ClaimTypes.Name, userEntity.name);
        string token = _tokenProvider.GenerateToken(new List<Claim> { idClaim, emailClaim }.ToArray());
        return new CreateUserResponse(userEntity.id, userEntity.name, userEntity.email, userEntity.defaultCurrency,
            token);
    }
}