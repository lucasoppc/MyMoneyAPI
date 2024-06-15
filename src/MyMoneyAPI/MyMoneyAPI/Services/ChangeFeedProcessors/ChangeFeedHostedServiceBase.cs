using Microsoft.Azure.Cosmos;
using MyMoneyAPI.Services.CosmosDB;

namespace MyMoneyAPI.Services.ChangeFeedProcessors;

public abstract class ChangeFeedHostedServiceBase<T>
    : IHostedService, IDisposable
{
    protected abstract Container Container { get; set; }
    protected abstract Container LeaseContainer { get; set; }
    protected abstract string InstanceName { get; set; }
    protected abstract string ChangeFeedProcessorName { get; set; }
    private ChangeFeedProcessor _changeFeedProcessor;
    

    private ChangeFeedProcessor GetChangeFeedProcessor()
    {
        return Container.GetChangeFeedProcessorBuilder(ChangeFeedProcessorName,
                async Task (IReadOnlyCollection<T> documents, CancellationToken cancellationToken) => await OnChangeHandler(documents, cancellationToken))
            .WithInstanceName(InstanceName)
            .WithLeaseContainer(LeaseContainer)
            .Build();
    }

    protected abstract Task OnChangeHandler(IReadOnlyCollection<T> documents, CancellationToken cancellationToken);

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _changeFeedProcessor = GetChangeFeedProcessor();
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
}