using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Enumeration;
using TaxCalculation.Core.Model;
using TaxCalculation.Core.Strategy;

namespace TaxCalculation.Persistent.Calculator
{
    public class TaxJar : ITaxCalculatorStrategy
    {

        public bool IsApplicable(TaxCalculatorOption option)
        {
            return option == TaxCalculatorOption.Tax_Jar;
        }

        public Task<Tax> CalculateTax(Order data)
        {
            throw new NotImplementedException();
        }

        public Task<Rate> GetTaxRateByLocation(Location data)
        {
            throw new NotImplementedException();
        }

       
    }
}
