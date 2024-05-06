using MyMoneyAPI.Features.Transactions.Models;

namespace MyMoneyAPI.Features.Transactions.Responses;

public class GetTransactionsResponse
{
    public int Total => Transactions.Count;
    public List<Transaction> Transactions { get; }

    public GetTransactionsResponse(List<Transaction> transactions)
    {
        Transactions = transactions ?? [];
    }
}