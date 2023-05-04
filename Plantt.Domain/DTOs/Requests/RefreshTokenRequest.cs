namespace Plantt.Domain.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        public required Guid AccountPublicId { get; set; }
        public required string Token { get; set; }
    }
}
