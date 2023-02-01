namespace VNK.Omega.Exceptions;

public class ScheduleTimeParseException : Exception
{
    public ScheduleTimeParseException(string message): base(message)
    { }
    
    public ScheduleTimeParseException(string message, Exception innerException): base(message, innerException)
    { }
}