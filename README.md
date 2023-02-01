# OmegaScheduler

## Installation
``dotnet add package Omega --version 1.0.0``

## Usage
### Worker Class
#### With TimeOnly
    using VNK.Omega.Workers;

    public class WorkerClass1 : Worker
    {
        public override string Name => "Worker 1"
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


#### With string
    using VNK.Omega.Workers;

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

### Dependency Injection
#### Add Omega Server
    #Program.cs
    builder.Services.AddOmega(setup =>
    {
        //default values
        setup.EnableLogin = true;
        setup.Retries = 3;
        setup.MinutesToRetry = 5;
    });

#### Add Workers
    #Program.cs
    builder.Services.AddWorker<WorkerClass1>();
    builder.Services.AddWorker<WorkerClass2>();