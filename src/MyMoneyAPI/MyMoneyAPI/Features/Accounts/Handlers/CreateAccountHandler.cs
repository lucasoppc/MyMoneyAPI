using MediatR;
using MyMoneyAPI.Features.Accounts.Models;
using MyMoneyAPI.Features.Accounts.Repositories;
using MyMoneyAPI.Features.Accounts.Requests;
using MyMoneyAPI.Features.Accounts.Responses;

namespace MyMoneyAPI.Features.Accounts.Handlers;

public class CreateAccountHandler : IRequestHandler<CreateAccountRequest, CreateAccountResponse>
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<CreateAccountResponse> Handle(CreateAccountRequest request, CancellationToken cancellationToken = default)
    {
        var newAccount = new Account
        {
            id = Guid.NewGuid().ToString(),
            name = request.Name,
            userId = "debugUserID",
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