using MediatR;
using MyMoneyAPI.Features.Accounts.Models;
using MyMoneyAPI.Features.Accounts.Repositories;
using MyMoneyAPI.Features.Accounts.Requests;
using MyMoneyAPI.Features.Accounts.Responses;

namespace MyMoneyAPI.Features.Accounts.Handlers;

public class CreateAccountHandler : IRequestHandler<CreateAccountRequest, CreateAccountResponse>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public CreateAccountHandler(IAccountRepository accountRepository, IHttpContextAccessor httpContextAccessor)
    {
        _accountRepository = accountRepository;
        _contextAccessor = httpContextAccessor;

    }
    
    public async Task<CreateAccountResponse> Handle(CreateAccountRequest request, CancellationToken cancellationToken = default)
    {
        var userId = _contextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (string.IsNullOrWhiteSpace(userId?.Value))
        {
            throw new Exception("Missing user id");
        }
        var newAccount = new Account
        {
            id = Guid.NewGuid().ToString(),
            name = request.Name,
            userId = userId.Value,
            currency = request.Currency,
            isDeleted = false
        };
        
        var result = await _accountRepository.CreateAccountAsync(newAccount);

        return new CreateAccountResponse
        {
            Id = result.id,
            Name = result.name,
            Currency = result.currency
        };
    }
}