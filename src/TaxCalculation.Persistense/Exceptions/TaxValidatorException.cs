using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculation.Persistent.Exceptions
{
    public class TaxValidatorException : Exception
    {
        public TaxValidatorException() { }
        public TaxValidatorException(string message) : base(message) { }
        public TaxValidatorException(string message, Exception inner) : base(message, inner) { }
    }
}
