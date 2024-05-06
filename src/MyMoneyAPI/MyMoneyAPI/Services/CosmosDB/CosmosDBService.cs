using Microsoft.Azure.Cosmos;

namespace MyMoneyAPI.Services.CosmosDB;

public class CosmosDBService : ICosmosDBService
{
    private readonly CosmosClient _cosmosClient;

    public CosmosDBService(CosmosClient client)
    {
        _cosmosClient = client;
        TransactionsContainer = _cosmosClient.GetContainer("MyMoney", "Transactions");
        AccountsContainer = _cosmosClient.GetContainer("MyMoney", "Accounts");
        TransactionsLeasesContainer = _cosmosClient.GetContainer("MyMoney", "TransactionsLeases");
    }

    public Container TransactionsContainer { get; }
    public Container AccountsContainer { get; }
    public Container TransactionsLeasesContainer { get; }
}