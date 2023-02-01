namespace VNK.Omega.Interfaces;

internal interface IOmegaLogger
{
    void LogInformation(string message);

    void LogError(Exception? exception, string message, params object?[] args);
}