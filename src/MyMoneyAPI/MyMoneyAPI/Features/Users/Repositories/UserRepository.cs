using Microsoft.Azure.Cosmos;
using MyMoneyAPI.Services.CosmosDB;
using Microsoft.Azure.Cosmos.Linq;
using MyMoneyAPI.Features.Users.Models;

namespace MyMoneyAPI.Features.Users.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ICosmosDBService _cosmosDbService;

    public UserRepository(ICosmosDBService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }
    
    public async Task<UserEntity> CreateUserAsync(UserEntity userEntity)
    {
        return await _cosmosDbService.UsersContainer.CreateItemAsync(userEntity);
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        var query = _cosmosDbService.UsersContainer.GetItemLinqQueryable<Models.UserEntity>()
            .Where(u => u.email == email)
            .ToFeedIterator();
        
        while (query.HasMoreResults)
        {
            var items = await query.ReadNextAsync();
            return items.FirstOrDefault();
        }

        return null;
    }
    
    public async Task<UserEntity> GetUserByIdAsync(string userId)
    {
        return await _cosmosDbService.UsersContainer.ReadItemAsync<Models.UserEntity>(userId, new PartitionKey(userId));
    }
}