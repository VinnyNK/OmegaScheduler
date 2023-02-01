using VNK.Omega.Extensions;
using VNK.Omega.Interfaces;

namespace VNK.Omega.Scheduler;

internal class TaskQueue : ITaskQueue
{
    private readonly List<IWorker> _queue;
    private readonly IOmegaLogger _logger;

    public TaskQueue(IOmegaLogger logger)
    {
        _logger = logger;
        _queue = new List<IWorker>();
    }

    public void AddWorker(IWorker workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }

        _logger.LogInformation($"Adding worker {workItem.Name} to queue");
        _queue.Add(workItem);
        _queue.Sort((x, y) => DateTime.Compare(x.NextRun, y.NextRun));
        _logger.LogInformation($"Sorted workers {Environment.NewLine}{string.Join(Environment.NewLine, _queue.Select(x => $"* {x.Name} - {x.NextRun}"))}");
    }

    public IWorker DequeueWorker()
    {
        var workItem = _queue.Pop();
        _logger.LogInformation($"Removing worker {workItem.Name} from queue");

        return workItem;
    }

    public IWorker PeekNextWorker() => _queue.Peek();
}