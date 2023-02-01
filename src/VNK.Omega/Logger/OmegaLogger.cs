using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VNK.Omega.Interfaces;
using VNK.Omega.Options;

namespace VNK.Omega.Logger;

internal class OmegaLogger : IOmegaLogger
{
    private readonly ILogger<OmegaLogger> _logger;
    private readonly IOptions<OmegaServerSettings> _options;
    private const string Tag = "[Omega]";

    public OmegaLogger(ILogger<OmegaLogger> logger, IOptions<OmegaServerSettings> options)
    {
        _logger = logger;
        _options = options;
    }

    public void LogInformation(string message)
    {
        if (!_options.Value.EnableLogin) return;;
        var date = $"[{DateTime.Now}]";
        _logger.LogInformation($"{date} {Tag} - {message}");
    }

    public void LogError(Exception? exception, string message, params object?[] args)
    {
        if (_options.Value.EnableLogin)
            _logger.LogError(exception, $"{Tag} - {message}", args);
    }
}