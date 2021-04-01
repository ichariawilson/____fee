using System;

namespace Scholarship.API.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class ScholarshipDomainException : Exception
    {
        public ScholarshipDomainException()
        { }

        public ScholarshipDomainException(string message)
            : base(message)
        { }

        public ScholarshipDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
