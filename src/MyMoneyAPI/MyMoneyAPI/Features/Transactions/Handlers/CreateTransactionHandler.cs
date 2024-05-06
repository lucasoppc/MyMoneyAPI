using MediatR;
using MyMoneyAPI.Features.Transactions.Models;
using MyMoneyAPI.Features.Transactions.Repositories;
using MyMoneyAPI.Features.Transactions.Requests;
using MyMoneyAPI.Features.Transactions.Responses;

namespace MyMoneyAPI.Features.Transactions.Handlers;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionRequest, CreateTransactionResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    
    public CreateTransactionHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<CreateTransactionResponse> Handle(CreateTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var newTransaction = new Transaction
        {
            id = Guid.NewGuid().ToString(),
            accountId = request.AccountId,
            userId = "debugUserID",
            description = request.Description,
            amount = request.Amount,
            currency = request.Currency,
            date = request.Date.ToLongDateString()
        };

        var createdTransaction = await _transactionRepository.CreateTransactionAsync(newTransaction);
        
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