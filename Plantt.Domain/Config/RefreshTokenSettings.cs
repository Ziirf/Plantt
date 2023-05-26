namespace Plantt.Domain.Config
{
    public class RefreshTokenSettings
    {
        public int RefreshTokenLength { get; set; }
        public int RefreshFamilyLength { get; set; }
        public TimeToLiveSettings TimeToLive { get; set; } = default!;
    }
}
