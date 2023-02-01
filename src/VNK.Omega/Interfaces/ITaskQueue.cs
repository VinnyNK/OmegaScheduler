namespace VNK.Omega.Interfaces;

internal interface ITaskQueue
{
    void AddWorker(IWorker workItem);

    IWorker DequeueWorker();

    IWorker PeekNextWorker();
}