using VNK.Omega.Exceptions;
using VNK.Omega.Interfaces;

namespace VNK.Omega.Workers;

public abstract class Worker : IWorker
{
    public abstract string Name { get; }
    public IEnumerable<TimeOnly> ScheduledTimes { get; }
    public DateTime NextRun { get; private set; }
    public DateTime LastRun { get; private set; }
    public int Retries { get; private set; }

    protected Worker(IEnumerable<TimeOnly> scheduledTimes)
    {
        var times = scheduledTimes.ToList();
        times.Sort((x, y) => x.CompareTo(y));
        ScheduledTimes = times;

        Retries = 0;
        
        Validate();
    }

    protected Worker(string[] scheduledTimes)
    {
        try
        {
            var times = scheduledTimes.Select(TimeOnly.Parse).ToList();
            times.Sort((x, y) => x.CompareTo(y));
            ScheduledTimes = times;

            Retries = 0;
        }
        catch (FormatException e)
        {
            throw new ScheduleTimeParseException(e.Message, e);
        }
        
        Validate();
    }
    
    public abstract Task RunAsync(CancellationToken cancellationToken);
    
    void IWorker.SetNextRun()
    {
        var date = DateTime.Now;
        if (LastRun == DateTime.MinValue)
        {
            var time = ScheduledTimes.FirstOrDefault(x => x >= TimeOnly.FromDateTime(date));
            NextRun = 
                time == TimeOnly.MinValue 
                    ? date.Date.AddDays(1).Add(ScheduledTimes.First().ToTimeSpan()) 
                    : date.Date.Add(time.ToTimeSpan());
            return;
        }
            
        if (ScheduledTimes.Last() < TimeOnly.FromDateTime(date) && LastRun.Day == date.Day)
        {
            NextRun = date.Date.AddDays(1).Add(ScheduledTimes.First().ToTimeSpan());
            return;
        }

        foreach (var time in ScheduledTimes)
        {
            if (time < TimeOnly.FromDateTime(date)) continue;
            NextRun = date.Date.Add(time.ToTimeSpan());
            return;
        }

        NextRun = date.Date.Add(ScheduledTimes.First().ToTimeSpan());
    }

    void IWorker.SetNextRunRetry(int minutes)
    {
        Retries += 1;
        NextRun = NextRun.AddMinutes(minutes);
    }

    void IWorker.SetLastRun(DateTime executedTime)
    {
        LastRun = executedTime;
    }

    public void Validate()
    {       
        if (string.IsNullOrEmpty(Name)) throw new WorkerNameNullOrEmptyException("Worker name cannot be blank or empty");
        if (!ScheduledTimes.Any()) throw new ScheduleTimeEmptyException("Schedule Time cannot be empty");
    }
}