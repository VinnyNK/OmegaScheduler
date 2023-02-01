namespace VNK.Omega.Interfaces;

internal interface IWorker
{
    public string Name { get; }
    public IEnumerable<TimeOnly> ScheduledTimes { get; }
    public DateTime NextRun { get; }
    public DateTime LastRun { get; }
    public int Retries { get; }

    public Task RunAsync(CancellationToken cancellationToken);

    internal void SetNextRun();

    internal void SetNextRunRetry(int minutes);

    internal void SetLastRun(DateTime executedTime);

    public void Validate();
}