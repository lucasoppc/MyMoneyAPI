using MediatR;
using MyMoneyAPI.Common.Exceptions;
using MyMoneyAPI.Features.Accounts.Repositories;
using MyMoneyAPI.Features.Transactions.Events;
using MyMoneyAPI.Features.Transactions.Models;
using MyMoneyAPI.Features.Transactions.Repositories;

namespace MyMoneyAPI.Features.Transactions.Handlers;

public class TransferEventHandler(ILogger<TransferEventHandler> logger,
    IAccountRepository accountRepository,
    ITransactionRepository transactionRepository,
    IMediator mediator) 
    : INotificationHandler<TransferEvent>
{
    public async Task Handle(TransferEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var senderTransaction = notification.Transaction;
            
            var toAccount = await accountRepository.GetUserAccountAsync(senderTransaction.transferDetails.toAccountId);
            if (toAccount is null)
            {
                throw new InvalidUserInputException($"Account {senderTransaction.transferDetails.toAccountId} not found");
            }
            
            var transaction = new Transaction
            {
                accountId = senderTransaction.transferDetails.toAccountId,
                userId = senderTransaction.userId,
                isTransference = false,
                amount = senderTransaction.amount * -1,
                currency = senderTransaction.currency,
                date = DateTime.UtcNow.ToString("O"),
                description = "Transference from account " + senderTransaction.accountId,
                id = Guid.NewGuid().ToString()
            };
            
            await transactionRepository.CreateTransactionAsync(transaction);
        }
        catch (Exception e)
        {
            await mediator.Publish(new TransactionFailedEvent(e.Message, notification.Transaction), cancellationToken);
            
            logger.LogError("Error while processing transfer event: {0}", e.Message);
            logger.LogError("Transference from account {0} to account {1} failed. Rolling back transaction...", 
                notification.Transaction.accountId, 
                notification.Transaction.transferDetails.toAccountId);
            throw;
        }
    }
}