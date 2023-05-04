namespace Plantt.Domain.Models
{
    public class Password
    {
        public required byte[] HashedPassword { get; init; }
        public required byte[] Salt { get; init; }
        public required int Iterations { get; init; }
    }
}
