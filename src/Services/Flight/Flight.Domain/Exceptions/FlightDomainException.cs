using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Domain.Exceptions
{
    public class FlightDomainException : Exception
    {
        public FlightDomainException()
        { }

        public FlightDomainException(string message)
            : base(message)
        { }

        public FlightDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
