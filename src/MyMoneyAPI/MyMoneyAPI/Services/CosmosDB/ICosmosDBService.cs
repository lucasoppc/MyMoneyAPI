using Microsoft.Azure.Cosmos;

namespace MyMoneyAPI.Services.CosmosDB;

public interface ICosmosDBService
{
    Container TransactionsContainer { get; }
    Container AccountsContainer { get; }
    Container TransactionsLeasesContainer { get; }
}