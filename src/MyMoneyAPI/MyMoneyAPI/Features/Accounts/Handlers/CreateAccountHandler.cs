using MediatR;
using MyMoneyAPI.Common.Constants;
using MyMoneyAPI.Common.Exceptions;
using MyMoneyAPI.Features.Accounts.Models;
using MyMoneyAPI.Features.Accounts.Repositories;
using MyMoneyAPI.Features.Accounts.Requests;
using MyMoneyAPI.Features.Accounts.Responses;

namespace MyMoneyAPI.Features.Accounts.Handlers;

public class CreateAccountHandler(IAccountRepository accountRepository,
    IHttpContextAccessor contextAccessor) 
    : IRequestHandler<CreateAccountRequest, CreateAccountResponse>
{
    
    public async Task<CreateAccountResponse> Handle(CreateAccountRequest request, CancellationToken cancellationToken = default)
    {
        var userId = contextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (string.IsNullOrWhiteSpace(userId?.Value))
        {
            throw new InvalidUserInputException("Missing user id");
        }

        var currency = request.Currency;
        if (!CurrencyConstants.IsValidCurrency(ref currency))
        {
            throw new InvalidUserInputException("Invalid currency");
        }
        
        var newAccount = new Account
        {
            id = Guid.NewGuid().ToString(),
            name = request.Name,
            bankAccount = request.BankAccount,
            userId = userId.Value,
            currency = currency,
            isDeleted = false
        };
        
        var result = await accountRepository.CreateAccountAsync(newAccount);

        return new CreateAccountResponse(result);
    }
}