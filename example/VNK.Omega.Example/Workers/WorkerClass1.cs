using VNK.Omega.Workers;

namespace VNK.Omega.Example.Workers;

public class WorkerClass1 : Worker
{
    public override string Name => "Worker 1";

    private readonly ILogger<WorkerClass1> _logger;

    private new static readonly IEnumerable<TimeOnly> ScheduledTimes = new List<TimeOnly>()
    {
        new TimeOnly(00, 00, 00),
        new TimeOnly(10, 00, 00),
        new TimeOnly(18, 00, 00)
    };

    public WorkerClass1(ILogger<WorkerClass1> logger) : base(ScheduledTimes)
    {
        _logger = logger;
    }
    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Worker 1");
        await Task.Delay(5_000, cancellationToken);
        _logger.LogInformation("Ending Worker 1");
    }
}