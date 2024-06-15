using System.Net;
using MediatR;
using Microsoft.Azure.Cosmos;
using MyMoneyAPI.Features.Accounts.Models;
using MyMoneyAPI.Features.Transactions.Events;
using MyMoneyAPI.Services.CosmosDB;

namespace MyMoneyAPI.Features.Transactions.Handlers;

public class TransactionAddedEventHandler(ILogger<TransactionAddedEventHandler> logger,
    ICosmosDBService cosmosDbService) 
    : INotificationHandler<TransactionAddedEvent>
{
    public async Task Handle(TransactionAddedEvent notification, CancellationToken cancellationToken)
    {
        var transaction = notification.transaction;
        
        logger.LogInformation("Trying to update account {0} with amount {1} from a new transaction...", transaction.accountId, transaction.amount);
                    
        var account = await cosmosDbService.AccountsContainer.ReadItemAsync<Account>(transaction.accountId,
            new PartitionKey(transaction.userId), cancellationToken: cancellationToken);
                    
        if(account.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogWarning("Account {0} not found, skipping transaction...", transaction.accountId);
            return;
        }
                    
        await cosmosDbService.AccountsContainer.PatchItemAsync<Account>(transaction.accountId, new PartitionKey(transaction.userId),
            new[] { PatchOperation.Set("/amount", (account.Resource.amount + transaction.amount))}, cancellationToken: cancellationToken);
                    
        logger.LogInformation("Successfully updated account {0} with amount {0} from a new transaction...", transaction.accountId, transaction.amount);
        
    }
}