using MyMoneyAPI.Features.Transactions.Models;

namespace MyMoneyAPI.Features.Transactions.Responses;

public class GetTransactionsResponse
{
    public int Total => Transactions.Count;
    public List<Transaction> Transactions { get; }

    public GetTransactionsResponse(List<Transaction> transactions)
    {
        Transactions = transactions;
    }
}

public class TransactionResponse
{
    public string Id { get; set; }
    public string AccountId { get; set; }
    public string UserId { get; set; }
    public string Description { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}