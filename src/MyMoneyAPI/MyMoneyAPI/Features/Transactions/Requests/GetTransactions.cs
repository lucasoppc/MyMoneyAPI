using MediatR;
using MyMoneyAPI.Features.Transactions.Responses;

namespace MyMoneyAPI.Features.Transactions.Requests;

public record GetTransactions(string AccountId) : IRequest<GetTransactionsResponse>;