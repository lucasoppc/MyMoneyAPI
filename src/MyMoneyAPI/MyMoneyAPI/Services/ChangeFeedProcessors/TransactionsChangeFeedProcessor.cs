using System.Collections.ObjectModel;
using System.Net;
using MediatR;
using Microsoft.Azure.Cosmos;
using MyMoneyAPI.Features.Accounts.Models;
using MyMoneyAPI.Features.Transactions.Events;
using MyMoneyAPI.Features.Transactions.Models;
using MyMoneyAPI.Services.CosmosDB;
namespace MyMoneyAPI.Services.ChangeFeedProcessors;

public class TransactionsChangeFeedProcessor 
    : ChangeFeedHostedServiceBase<Transaction>
{
    private readonly ICosmosDBService _cosmosDbService;
    private readonly IMediator _mediator;
    private readonly ILogger<TransactionsChangeFeedProcessor> _logger;
    
    protected sealed override Container Container { get; set; }
    protected sealed override Container LeaseContainer { get; set; }
    protected sealed override string InstanceName { get; set; }
    protected sealed override string ChangeFeedProcessorName { get; set; }

    public TransactionsChangeFeedProcessor(ICosmosDBService cosmosDbService, 
        IMediator mediator,
        ILogger<TransactionsChangeFeedProcessor> logger)
    {
        _cosmosDbService = cosmosDbService;
        _mediator = mediator;
        _logger = logger;
        Container = _cosmosDbService.TransactionsContainer;
        LeaseContainer = _cosmosDbService.TransactionsLeasesContainer;
        InstanceName = "MyMoneyAPI";
        ChangeFeedProcessorName = "Transactions Change Feed Processor";
    }

    protected override async Task OnChangeHandler(IReadOnlyCollection<Transaction> documents, CancellationToken cancellationToken)
    {
        try
        {
            List<Task> tasks = [];
            
            foreach (var transaction in documents)
            {
                tasks.Add(_mediator.Publish(new TransactionAddedEvent(transaction), cancellationToken));

                if (transaction.isTransference)
                {
                    tasks.Add(_mediator.Publish(new TransferEvent(transaction), cancellationToken));
                }
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while processing transactions change feed: {0}", ex.Message);
        }
    }
}