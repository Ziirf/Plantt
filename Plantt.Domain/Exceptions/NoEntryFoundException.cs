namespace Plantt.Domain.Exceptions
{
    public class NoEntryFoundException : Exception
    {
        public NoEntryFoundException()
        {
        }

        public NoEntryFoundException(string message)
            : base(message)
        {
        }

        public NoEntryFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
