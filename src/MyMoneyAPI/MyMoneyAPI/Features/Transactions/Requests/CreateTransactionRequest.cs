using MediatR;
using MyMoneyAPI.Features.Transactions.Responses;

namespace MyMoneyAPI.Features.Transactions.Requests;

public record CreateTransactionRequest : IRequest<CreateTransactionResponse>
{
    public string AccountId { get; init; }
    public string Description { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    public DateTime Date { get; init; } = DateTime.UtcNow;
}