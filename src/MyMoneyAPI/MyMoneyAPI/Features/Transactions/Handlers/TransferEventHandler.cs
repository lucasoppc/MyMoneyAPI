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
        string toAccountName = string.Empty;
        
        try
        {
            var senderTransaction = notification.Transaction;
            
            var getAccountTo = accountRepository.GetUserAccountAsync(senderTransaction.transferDetails.toAccountId);
            var getAccountFrom = accountRepository.GetUserAccountAsync(senderTransaction.accountId);
            
            await Task.WhenAll(getAccountTo, getAccountFrom);
            
            var fromAccount = getAccountFrom.Result;
            var toAccount = getAccountTo.Result;
            toAccountName = toAccount.name;
            
            if (fromAccount is null)
            {
                throw new InvalidUserInputException($"Account {senderTransaction.accountId} not found");
            }
            if (toAccount is null)
            {
                throw new InvalidUserInputException($"Account {senderTransaction.transferDetails.toAccountId} not found");
            }
            
            var transaction = new Transaction
            {
                accountId = senderTransaction.transferDetails.toAccountId,
                userId = senderTransaction.userId,
                isTransference = false,
                amount = senderTransaction.amount * -1, // Reversing because the transaction is stores negative in the account from, so here we do credit.
                currency = senderTransaction.currency,
                date = DateTime.UtcNow.ToString("O"),
                description = "Transference received from: " + fromAccount.name,
                id = Guid.NewGuid().ToString()
            };
            
            await transactionRepository.CreateTransactionAsync(transaction);
        }
        catch (Exception e)
        {
            await mediator.Publish(new TransactionFailedEvent(e.Message, toAccountName, notification.Transaction), cancellationToken);
            
            logger.LogError("Error while processing transfer event: {0}", e.Message);
            logger.LogError("Transference from account {0} to account {1} failed. Rolling back transaction...", 
                notification.Transaction.accountId, 
                notification.Transaction.transferDetails.toAccountId);
            throw;
        }
    }
}