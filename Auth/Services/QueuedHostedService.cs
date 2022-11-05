namespace Auth.Services;

public class QueuedHostedService : BackgroundService
{
    private readonly ILogger<QueuedHostedService> _logger;

    public QueuedHostedService(IBackgroundTaskQueue taskQueue, ILogger<QueuedHostedService> logger)
    {
        TaskQueue = taskQueue;
        _logger = logger;
    }

    public IBackgroundTaskQueue TaskQueue { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"Queued Hosted Service is running.{Environment.NewLine}" +
            $"{Environment.NewLine}Tap W to add a work item to the " +
            $"background queue.{Environment.NewLine}");

        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Before loop [{Thread.CurrentThread.ManagedThreadId}]");
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await TaskQueue.DequeueAsync(stoppingToken);

            try
            {
                _logger.LogInformation($"Working [{Thread.CurrentThread.ManagedThreadId}]");
                await workItem(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred executing {nameof(workItem)}.");
            }
            _logger.LogInformation($"Waiting next [{Thread.CurrentThread.ManagedThreadId}]");
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queued Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
