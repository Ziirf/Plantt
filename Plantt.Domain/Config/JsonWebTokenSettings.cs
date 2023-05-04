using System.Text;

namespace Plantt.Domain.Config
{
    public class JsonWebTokenSettings
    {
        public string SecretKey { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public int MinutesToLive { get; set; }
        public byte[] SecretKeyBytes => Encoding.ASCII.GetBytes(SecretKey);
    }
}
