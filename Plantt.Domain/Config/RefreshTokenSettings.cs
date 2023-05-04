namespace Plantt.Domain.Config
{
    public class RefreshTokenSettings
    {
        public int RefreshTokenLength { get; set; }
        public int RefreshFamilyLength { get; set; }
        public int DaysToLive { get; set; }
    }
}
