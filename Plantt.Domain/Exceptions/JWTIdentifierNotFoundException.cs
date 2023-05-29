namespace Plantt.Domain.Exceptions
{
    public class JWTIdentifierNotFoundException : Exception
    {
        public JWTIdentifierNotFoundException()
        {
        }

        public JWTIdentifierNotFoundException(string message)
            : base(message)
        {
        }

        public JWTIdentifierNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
