namespace TakeSlot.Application.Options;

public class SlotServiceApiClientOptions
{
    public const string SlotServiceApiClient = nameof(SlotServiceApiClient);

    public Uri? BaseUrl { get; set; }
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}