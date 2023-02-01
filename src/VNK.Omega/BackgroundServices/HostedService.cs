using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using VNK.Omega.Interfaces;
using VNK.Omega.Options;

namespace VNK.Omega.BackgroundServices;

internal class HostedService : BackgroundService
{
    private readonly IOmegaLogger _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITaskQueue _taskQueue;
    private int Retries { get; }
    private readonly IOptions<OmegaServerSettings> _options;

    public HostedService(IOmegaLogger logger, IServiceProvider serviceProvider, ITaskQueue taskQueue, IOptions<OmegaServerSettings> options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _taskQueue = taskQueue;
        _options = options;
        Retries = _options.Value.Retries;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Omega server is running.");
        using var scope = _serviceProvider.CreateScope();
        var workers = scope.ServiceProvider.GetServices<IWorker>();
        foreach (var worker in workers)
        {
            worker.SetNextRun();
            _taskQueue.AddWorker(worker);
        }

        async void BackgroundCallback() => await BackgroundProcessing(stoppingToken);
            
        var hostApplicationLifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
        hostApplicationLifetime?.ApplicationStarted.Register(BackgroundCallback);
        return Task.CompletedTask;
    }
    
    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_taskQueue.PeekNextWorker().NextRun > DateTime.Now) continue;
            
            var workItem = _taskQueue.DequeueWorker();
            
            try
            {
                await workItem.RunAsync(stoppingToken);
                workItem.SetLastRun(workItem.NextRun);
                workItem.SetNextRun();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    $"Error occurred executing { workItem.Name }.", nameof(workItem));
                workItem.SetNextRunRetry(_options.Value.MinutesToRetry);
                if (workItem.Retries > Retries)
                {
                    workItem.SetNextRun();
                    _logger.LogInformation("Limit retries reached");
                } else _logger.LogInformation($"Count Retries: {workItem.Retries}");
            }
            _taskQueue.AddWorker(workItem);
        }
    }
    
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Omega server is stopping.");

        await base.StopAsync(stoppingToken);
    }
}