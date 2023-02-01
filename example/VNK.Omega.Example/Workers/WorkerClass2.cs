using VNK.Omega.Workers;

namespace VNK.Omega.Example.Workers;

public class WorkerClass2 : Worker
{
    public override string Name => "Worker 2";

    private readonly ILogger<WorkerClass2> _logger;

    private new static readonly string[] ScheduledTimes = new[]
    {
        "00:00:00",
        "10:00:00",
        "18:00:00"
    };
    
    public WorkerClass2(ILogger<WorkerClass2> logger) : base(ScheduledTimes)
    {
        _logger = logger;
    }
    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Worker 2");
        await Task.Delay(5_000, cancellationToken);
        _logger.LogInformation("Ending Worker 2");
    }
}