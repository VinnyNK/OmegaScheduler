using Microsoft.Extensions.Options;

namespace VNK.Omega.Options;

internal class OmegaServerConfigure : IConfigureOptions<OmegaServerSettings>
{
    public bool EnableLog { get; set; }
    
    public int Retries { get; set; }
    
    public int MinutesToRetry { get; set; }
    
    public void Configure(OmegaServerSettings options)
    {
        EnableLog = options.EnableLog;
        Retries = options.Retries;
        MinutesToRetry = options.MinutesToRetry;
    }
}