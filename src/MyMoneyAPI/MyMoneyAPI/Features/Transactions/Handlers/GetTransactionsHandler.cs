using MediatR;
using MyMoneyAPI.Features.Transactions.Repositories;
using MyMoneyAPI.Features.Transactions.Requests;
using MyMoneyAPI.Features.Transactions.Responses;

namespace MyMoneyAPI.Features.Transactions.Handlers;

public class GetTransactionsHandler : IRequestHandler<GetTransactions, GetTransactionsResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    
    public GetTransactionsHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<GetTransactionsResponse> Handle(GetTransactions request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetTransactionsAsync(request.AccountId);

        return new GetTransactionsResponse(transactions);
    }
}