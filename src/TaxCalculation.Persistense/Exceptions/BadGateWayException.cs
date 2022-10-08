using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculation.Persistent.Exceptions
{
    public class BadGateWayException : Exception
    {
        public BadGateWayException() { }
        public BadGateWayException(string message) : base(message) { }
        public BadGateWayException(string message, Exception inner) : base(message, inner) { }
    }
}
