using MediatR;
using MyMoneyAPI.Features.Transactions.Events;
using MyMoneyAPI.Features.Transactions.Models;
using MyMoneyAPI.Features.Transactions.Repositories;

namespace MyMoneyAPI.Features.Transactions.Handlers;

public class TransactionFailedEventHandler(ITransactionRepository transactionRepository)
    : INotificationHandler<TransactionFailedEvent>
{
    public async Task Handle(TransactionFailedEvent notification, CancellationToken cancellationToken)
    {
        var failedTransferenceTransaction = notification.Transaction;
        var creditTransaction = new Transaction
        {
            id = Guid.NewGuid().ToString(),
            accountId = failedTransferenceTransaction.accountId,
            userId = failedTransferenceTransaction.userId,
            amount = failedTransferenceTransaction.amount * -1,
            currency = failedTransferenceTransaction.currency,
            date = DateTime.UtcNow.ToString("O"),
            description = $"Failed transference to account {failedTransferenceTransaction.transferDetails.toAccountId}, reason: {notification.Reason}",
        };

        await transactionRepository.CreateTransactionAsync(creditTransaction);
    }
}