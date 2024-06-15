using MediatR;
using MyMoneyAPI.Common.Constants;
using MyMoneyAPI.Common.Exceptions;
using MyMoneyAPI.Features.Accounts.Models;
using MyMoneyAPI.Features.Accounts.Repositories;
using MyMoneyAPI.Features.Accounts.Requests;
using MyMoneyAPI.Features.Accounts.Responses;

namespace MyMoneyAPI.Features.Accounts.Handlers;

public class UpdateAccountHandler(IAccountRepository accountRepository,
    IHttpContextAccessor contextAccessor)
    : IRequestHandler<UpdateAccountRequest, UpdateAccountResponse>
{
    public async Task<UpdateAccountResponse> Handle(UpdateAccountRequest request, CancellationToken cancellationToken)
    {
        var userId = contextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (string.IsNullOrWhiteSpace(userId?.Value))
        {
            throw new InvalidUserInputException("Missing user id");
        }
        
        var existingAccount = await accountRepository.GetUserAccountAsync(request.Id);

        if (existingAccount is null || existingAccount.isDeleted)
        {
            throw new InvalidUserInputException("Account not found");
        }

        if (request.IsDeleted)
        {
            existingAccount.isDeleted = true;
        }
        else
        {
            Update(existingAccount, request);
        }

        await accountRepository.SaveAccountAsync(existingAccount);
        return new UpdateAccountResponse(existingAccount);
    }

    private void Update(Account existingAccount, UpdateAccountRequest request)
    {
        var currency = request.Currency;
        if (!CurrencyConstants.IsValidCurrency(ref currency))
        {
            throw new InvalidUserInputException("Invalid currency");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new InvalidUserInputException("Name is required");
        }
        
        existingAccount.name = request.Name;
        existingAccount.bankAccount = request.BankAccount;
        existingAccount.currency = request.Currency;
        existingAccount.isDeleted = request.IsDeleted;
    }
}