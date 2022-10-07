using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Enumeration;
using TaxCalculation.Core.Model;

namespace TaxCalculation.Core.Strategy
{
    public interface ITaxCalculatorStrategy
    {
        bool IsApplicable(TaxCalculatorOption option);
        Task<Rate> GetTaxRateByLocation(Location data);
        Task<Tax> CalculateTax(Order data);
    }
}
