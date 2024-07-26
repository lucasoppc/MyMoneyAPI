using MediatR;
using MyMoneyAPI.Features.Transactions.Models;

namespace MyMoneyAPI.Features.Transactions.Events;

public record TransactionFailedEvent(string Reason, string ToAccountName, Transaction Transaction) : INotification
{
    
}