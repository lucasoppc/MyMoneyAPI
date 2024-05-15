using Microsoft.Azure.Cosmos;

namespace MyMoneyAPI.Services.CosmosDB.Configuration;

public static class CosmosDBConfigurationExtensions
{
    public static IServiceCollection AddCosmosDB(this IServiceCollection services, CosmosDBConfigOptions? configOptions)
    {
        services.AddSingleton<CosmosClient>(sp =>
        {
            var connectionString = Environment.GetEnvironmentVariable("COSMOS_DB_CONNECTION_STRING");
            var clientOptions = new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Direct,
                AllowBulkExecution = true,
                MaxRetryAttemptsOnRateLimitedRequests = 9,
                MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromSeconds(30)
        
            };
            return new CosmosClient(configOptions.ConnectionString);
        });
        
        services.AddSingleton<ICosmosDBService, CosmosDBService>();

        return services;
    }
}