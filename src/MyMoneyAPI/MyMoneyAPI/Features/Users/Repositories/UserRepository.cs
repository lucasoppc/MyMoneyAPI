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
    
    public async Task<UserEntity> CreateUserAsync(UserEntity userEntity, CancellationToken cancellationToken)
    {
        return await _cosmosDbService.UsersContainer.CreateItemAsync(userEntity, cancellationToken: cancellationToken);
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var query = _cosmosDbService.UsersContainer.GetItemLinqQueryable<Models.UserEntity>()
            .Where(u => u.email == email)
            .ToFeedIterator();
        
        while (query.HasMoreResults)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            var items = await query.ReadNextAsync(cancellationToken: cancellationToken);
            return items.FirstOrDefault();
        }

        return null;
    }
    
    public async Task<UserEntity> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _cosmosDbService.UsersContainer.ReadItemAsync<UserEntity>(userId, new PartitionKey(userId), cancellationToken: cancellationToken);
    }
}