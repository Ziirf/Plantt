using System.Text;

namespace Plantt.Domain.Config
{
    public class JsonWebTokenSettings
    {
        public string SecretKey { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public TimeToLiveSettings TimeToLive { get; set; } = default!;
        public byte[] SecretKeyBytes => Encoding.ASCII.GetBytes(SecretKey);
    }
}
