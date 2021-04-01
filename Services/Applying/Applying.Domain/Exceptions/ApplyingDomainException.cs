using System;

namespace Applying.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class ApplyingDomainException : Exception
    {
        public ApplyingDomainException()
        { }

        public ApplyingDomainException(string message)
            : base(message)
        { }

        public ApplyingDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
