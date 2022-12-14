using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Enumeration;

namespace TaxCalculation.Core.Model
{
    public class Location
    {
        public string Country { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string TaxCalculatorOption { get; set; }
    }
}
