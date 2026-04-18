namespace VNK.Omega.Options;

public class OmegaServerSettings
{
    public bool EnableLog { get; set; } = true;

    public int Retries { get; set; } = 3;

    public int MinutesToRetry { get; set; } = 5;
}