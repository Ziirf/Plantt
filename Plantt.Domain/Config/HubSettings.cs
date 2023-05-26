namespace Plantt.Domain.Config
{
    public class HubSettings
    {
        public TimeToLiveSettings TokenTimeToLive { get; set; } = default!;
    }
}
