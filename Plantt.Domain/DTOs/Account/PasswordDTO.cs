namespace Plantt.Domain.DTOs.Account
{
    public class PasswordDTO
    {
        public required byte[] HashedPassword { get; init; }
        public required byte[] Salt { get; init; }
        public required int Iterations { get; init; }
    }
}
