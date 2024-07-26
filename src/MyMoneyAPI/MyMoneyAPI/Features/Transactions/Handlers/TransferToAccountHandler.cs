using Azure.Core;
using MediatR;
using MyMoneyAPI.Common.Exceptions;
using MyMoneyAPI.Features.Accounts.Repositories;
using MyMoneyAPI.Features.Transactions.Models;
using MyMoneyAPI.Features.Transactions.Repositories;
using MyMoneyAPI.Features.Transactions.Requests;
using MyMoneyAPI.Features.Transactions.Responses;

namespace MyMoneyAPI.Features.Transactions.Handlers;

public class TransferToAccountHandler(IAccountRepository accountRepository,
    ITransactionRepository transactionRepository,
    IHttpContextAccessor contextAccessor)
    : IRequestHandler<TransferToAccountRequest, TransferToAccountResponse>
{
    public async Task<TransferToAccountResponse> Handle(TransferToAccountRequest request, CancellationToken cancellationToken)
    {
        var userId = contextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (string.IsNullOrWhiteSpace(userId?.Value))
        {
            throw new InvalidUserInputException("Missing user id");
        }

        var getAccountFrom = accountRepository.GetUserAccountAsync(request.FromAccountId);
        var getAccountTo = accountRepository.GetUserAccountAsync(request.ToAccountId);

        await Task.WhenAll(getAccountFrom, getAccountTo);
        
        var accountFrom = getAccountFrom.Result;
        var accountTo = getAccountTo.Result;
        
        if (accountFrom is null || accountFrom.userId != userId.Value)
        {
            throw new InvalidUserInputException("Account doesn't exists or doesn't belong to the user");
        }
        
        if (accountTo is null || accountTo.userId != userId.Value)
        {
            throw new InvalidUserInputException("Account doesn't exists or doesn't belong to the user");
        }
        
        if(accountFrom.amount < request.Amount)
        {
            throw new InvalidUserInputException("Insufficient funds");
        }

        var transaction = new Transaction
        {
            accountId = request.FromAccountId,
            userId = userId.Value,
            description = "Transference to: " + accountTo.name,
            amount = request.Amount * -1,
            date = DateTime.UtcNow.ToString("O"),
            id = Guid.NewGuid().ToString(),
            currency = accountFrom.currency,
            isTransference = true,
            transferDetails = new TransferDetails
            {
                toAccountId = request.ToAccountId
            }
        };

        var result = await transactionRepository.CreateTransactionAsync(transaction);
        return new TransferToAccountResponse(result);
    }
}