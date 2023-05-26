namespace Plantt.Domain.Config
{
    public class TimeToLiveSettings
    {
        public int Days { get; set; } = 0;
        public int Hours { get; set; } = 0;
        public int Minutes { get; set; } = 0;
        public int Seconds { get; set; } = 0;
        public TimeSpan Time => new TimeSpan(Days, Hours, Minutes, Seconds);
    }
}
