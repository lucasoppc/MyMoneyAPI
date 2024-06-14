using System.Globalization;
using MediatR;
using MyMoneyAPI.Common.Exceptions;
using MyMoneyAPI.Features.Accounts.Repositories;
using MyMoneyAPI.Features.Transactions.Models;
using MyMoneyAPI.Features.Transactions.Repositories;
using MyMoneyAPI.Features.Transactions.Requests;
using MyMoneyAPI.Features.Transactions.Responses;

namespace MyMoneyAPI.Features.Transactions.Handlers;

public class CreateTransactionHandler(ITransactionRepository transactionRepository,
    IAccountRepository accountsRepository,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<CreateTransactionRequest, CreateTransactionResponse>
{
    
    public async Task<CreateTransactionResponse> Handle(CreateTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (string.IsNullOrWhiteSpace(userId?.Value))
        {
            throw new InvalidUserInputException("Missing user id");
        }
        
        var account = await accountsRepository.GetUserAccountsAsync(userId.Value);
        if(!account.Any(a => a.id == request.AccountId && !a.isDeleted))
        {
            throw new InvalidUserInputException("Account doesn't exists, is deleted or doesn't belong to the user");
        }
        
        var newTransaction = new Transaction
        {
            id = Guid.NewGuid().ToString(),
            accountId = request.AccountId,
            userId = userId.Value,
            description = request.Description,
            amount = request.Amount,
            currency = request.Currency,
            date = request.Date.ToString("O")
        };

        var createdTransaction = await transactionRepository.CreateTransactionAsync(newTransaction);
        
        return new CreateTransactionResponse
        {
            AccountId = createdTransaction.accountId,
            UserId = createdTransaction.userId,
            Description = createdTransaction.description,
            Amount = createdTransaction.amount,
            Currency = createdTransaction.currency,
            Date = DateTime.Parse(createdTransaction.date)
        };
    }
}