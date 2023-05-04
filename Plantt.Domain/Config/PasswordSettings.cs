namespace Plantt.Domain.Config
{
    public class PasswordSettings
    {
        public int SaltSize { get; set; }
        public int MinIterations { get; set; }
        public int MaxIterations { get; set; }
        public int PasswordHashSize { get; set; }
    }
}
