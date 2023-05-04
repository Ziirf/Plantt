namespace Plantt.Domain.DTOs.Account
{
    public class AccountDto
    {
        public required Guid PublicId { get; set; }

        public required string Username { get; set; }

        public required string Email { get; set; }
    }
}
