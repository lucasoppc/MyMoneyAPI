namespace MyMoneyAPI.Features.Transactions.Responses;

public record CreateTransactionResponse
{
    public string Description { get; init; }
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public string AccountId { get; init; }
    public string UserId { get; init; }
    public string Currency { get; init; }
}