using System;
using System.Runtime.Serialization;

namespace AccountManager.Domain.Exceptions
{
    public class AccountManagerDomainException : Exception
    {
        public AccountManagerDomainException()
        {
        }

        public AccountManagerDomainException(string message) : base(message)
        {
        }

        public AccountManagerDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AccountManagerDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
