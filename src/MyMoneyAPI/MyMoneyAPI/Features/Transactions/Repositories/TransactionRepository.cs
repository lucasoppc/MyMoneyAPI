using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using MyMoneyAPI.Services.CosmosDB;
using MyMoneyAPI.Features.Transactions.Models;

namespace MyMoneyAPI.Features.Transactions.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ICosmosDBService _cosmosDbService;

    public TransactionRepository(ICosmosDBService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }

    public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
    {
        return await _cosmosDbService.TransactionsContainer.CreateItemAsync(transaction);
    }
    
    public async Task<List<Transaction>> GetTransactionsAsync(string accountId)
    {
        var results = new List<Transaction>();
        var query = _cosmosDbService.TransactionsContainer.GetItemLinqQueryable<Transaction>()
            .Where(t => t.accountId == accountId)
            .ToFeedIterator();

        while (query.HasMoreResults)
        {
            var items = await query.ReadNextAsync();
            results.AddRange(items.Select(item => (item)));
        }

        return results;
    }
}