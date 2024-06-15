using MediatR;
using MyMoneyAPI.Features.Transactions.Models;

namespace MyMoneyAPI.Features.Transactions.Events;

public record TransactionAddedEvent(Transaction transaction) : INotification
{
    
}