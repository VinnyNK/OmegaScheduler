namespace VNK.Omega.Options;

public class OmegaServerSettings
{
    public bool EnableLogin { get; set; } = true;

    public int Retries { get; set; } = 3;

    public int MinutesToRetry { get; set; } = 5;
}