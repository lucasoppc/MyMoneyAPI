using Microsoft.Azure.Cosmos;
using MyMoneyAPI.Features.Transactions.Models;
using MyMoneyAPI.Services.CosmosDB;

namespace MyMoneyAPI.Services.ChangeFeedProcessors;

public class TransactionsChangeFeedProcessor : IHostedService, IDisposable
{
    private ICosmosDBService _cosmosDBService;
    private ChangeFeedProcessor _changeFeedProcessor;

    public TransactionsChangeFeedProcessor(ICosmosDBService cosmosDBService)
    {
        _cosmosDBService = cosmosDBService;
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
                (IReadOnlyCollection<Transaction> transactions, CancellationToken cancellationToken = default) =>
                {
                    foreach (var transaction in transactions)
                    {
                        Console.WriteLine("Transaction added: " + transaction.amount);
                    }

                    return Task.CompletedTask;
                })
            .WithInstanceName("MyMoneyAPI")
            .WithLeaseContainer(_cosmosDBService.TransactionsLeasesContainer)
            .Build();
    }
}