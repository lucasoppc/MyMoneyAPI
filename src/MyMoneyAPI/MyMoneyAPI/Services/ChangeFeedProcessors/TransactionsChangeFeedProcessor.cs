using System.Collections.ObjectModel;
using System.Net;
using Microsoft.Azure.Cosmos;
using MyMoneyAPI.Features.Accounts.Models;
using MyMoneyAPI.Features.Transactions.Models;
using MyMoneyAPI.Services.CosmosDB;
namespace MyMoneyAPI.Services.ChangeFeedProcessors;

public class TransactionsChangeFeedProcessor : IHostedService, IDisposable
{
    private ICosmosDBService _cosmosDBService;
    private ChangeFeedProcessor _changeFeedProcessor;
    private ILogger<TransactionsChangeFeedProcessor> _logger;

    public TransactionsChangeFeedProcessor(ICosmosDBService cosmosDBService, ILogger<TransactionsChangeFeedProcessor> logger)
    {
        _cosmosDBService = cosmosDBService;
        _logger = logger;
        _changeFeedProcessor = GetChangeFeedProcessor();
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _changeFeedProcessor.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _changeFeedProcessor.StopAsync();
    }
    
    public void Dispose()
    {
        _changeFeedProcessor.StopAsync().Wait();
    }
    
    private ChangeFeedProcessor GetChangeFeedProcessor()
    {
        return _cosmosDBService.TransactionsContainer
            .GetChangeFeedProcessorBuilder("TransactionsChangeFeedProcessor",
                async Task (IReadOnlyCollection<Transaction> transactions, CancellationToken cancellationToken = default) =>
                {
                    foreach (var transaction in transactions)
                    {
                        _logger.LogInformation("Trying to update account @accountId with amount @amount from a new transaction...", transaction.accountId, transaction.amount);
                        
                        var account = await _cosmosDBService.AccountsContainer.ReadItemAsync<Account>(transaction.accountId,
                            new PartitionKey(transaction.userId), cancellationToken: cancellationToken);
                        
                        if(account.StatusCode == HttpStatusCode.NotFound)
                        {
                            _logger.LogWarning("Account @accountId not found, skipping transaction...", transaction.accountId);
                            return;
                        }
                        
                        await _cosmosDBService.AccountsContainer.PatchItemAsync<Account>(transaction.accountId, new PartitionKey(transaction.userId),
                            new[] { PatchOperation.Set("/amount", (account.Resource.amount + transaction.amount))}, cancellationToken: cancellationToken);
                        
                        _logger.LogInformation("Successfully updated account @accountId with amount @amount from a new transaction...", transaction.accountId, transaction.amount);
                    }
                })
            .WithInstanceName("MyMoneyAPI")
            .WithLeaseContainer(_cosmosDBService.TransactionsLeasesContainer)
            .Build();
    }
}