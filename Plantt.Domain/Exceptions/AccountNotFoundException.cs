using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Domain.Exceptions
{
    public class AccountNotFoundException : Exception
    {

        public AccountNotFoundException()
        {
        }

        public AccountNotFoundException(string message)
            : base(message)
        {
        }

        public AccountNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
