using Microsoft.Extensions.Options;

namespace VNK.Omega.Options;

internal class OmegaServerConfigure : IConfigureOptions<OmegaServerSettings>
{
    public bool EnableLogin { get; set; }
    
    public int Retries { get; set; }
    
    public int MinutesToRetry { get; set; }
    
    public void Configure(OmegaServerSettings options)
    {
        EnableLogin = options.EnableLogin;
        Retries = options.Retries;
        MinutesToRetry = options.MinutesToRetry;
    }
}