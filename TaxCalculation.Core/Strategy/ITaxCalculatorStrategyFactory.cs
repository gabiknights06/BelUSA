using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Model;

namespace TaxCalculation.Core.Strategy
{
    public interface ITaxCalculatorStrategyFactory
    {
        ITaxCalculatorStrategy Create(Order order);
    }
}
