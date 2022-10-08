using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculation.Persistent.Exceptions
{
    public class ValidatorException : Exception
    {
        public ValidatorException() { }
        public ValidatorException(string message) : base(message) { }
        public ValidatorException(string message, Exception inner) : base(message, inner) { }
    }
}
