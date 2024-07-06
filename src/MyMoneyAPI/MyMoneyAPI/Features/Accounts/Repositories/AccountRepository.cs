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
            .Where(a => a.userId == userId & !a.isDeleted)
            .ToFeedIterator();
        
        while (query.HasMoreResults)
        {
            var items = await query.ReadNextAsync();
            results.AddRange(items.Select(item => (item)));
        }

        return results;
    }
    
    public async Task<Account> GetUserAccountAsync(string accountId)
    {
        var result = new Account();
        var query = _cosmosDbService.AccountsContainer.GetItemLinqQueryable<Account>()
            .Where(a => a.id == accountId && !a.isDeleted)
            .ToFeedIterator();
        
        while (query.HasMoreResults)
        {
            var items = await query.ReadNextAsync();
            result = items.FirstOrDefault();
        }

        return result;
    }
    
    public async Task SaveAccountAsync(Account account)
    {
        var patchOperations = new List<PatchOperation>();
        var properties = typeof(Account).GetProperties();
        var excludedProperties = new List<string>{"id", "accountId", "userId"};

        foreach (var property in properties)
        {
            if(excludedProperties.Contains(property.Name))
            {
                continue;
            }
            var value = property.GetValue(account);
            var path = $"/{property.Name}";
            patchOperations.Add(PatchOperation.Replace(path, value));
        }

        await _cosmosDbService.AccountsContainer.PatchItemAsync<Account>(
            account.id,
            new PartitionKeyBuilder()
                .Add(account.userId)
                .Build(),
            patchOperations
        );
    }
}