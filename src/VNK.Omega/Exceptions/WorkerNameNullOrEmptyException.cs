namespace VNK.Omega.Exceptions;

public class WorkerNameNullOrEmptyException : Exception
{
    public WorkerNameNullOrEmptyException(string message): base(message)
    { }
}