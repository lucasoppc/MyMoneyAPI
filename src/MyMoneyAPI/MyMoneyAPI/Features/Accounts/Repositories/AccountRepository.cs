using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using MyMoneyAPI.Services.CosmosDB;
using MyMoneyAPI.Features.Accounts.Models;

namespace MyMoneyAPI.Features.Accounts.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ICosmosDBService _cosmosDbService;

    public AccountRepository(ICosmosDBService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }
    
    public async Task<Account> CreateAccountAsync(Account account)
    {
        return await _cosmosDbService.AccountsContainer.CreateItemAsync(account);
    }

    public async Task<List<Account>> GetUserAccountsAsync(string userId)
    {
        var results = new List<Account>();
        var query = _cosmosDbService.AccountsContainer.GetItemLinqQueryable<Account>()
            .Where(a => a.userId == userId)
            .ToFeedIterator();


        
        while (query.HasMoreResults)
        {
            var items = await query.ReadNextAsync();
            results.AddRange(items.Select(item => (item)));
        }

        return results;
    }
}