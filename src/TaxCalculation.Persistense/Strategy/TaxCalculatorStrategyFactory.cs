using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Enumeration;
using TaxCalculation.Core.Model;
using TaxCalculation.Core.Strategy;

namespace TaxCalculation.Persistent.Strategy
{
    public class TaxCalculatorStrategyFactory : ITaxCalculatorStrategyFactory
    {
        
        IEnumerable<ITaxCalculatorStrategy> _strategy;

        public TaxCalculatorStrategyFactory(IEnumerable<ITaxCalculatorStrategy> strategy)
        {
            _strategy = strategy;
        }

        public ITaxCalculatorStrategy Create(TaxCalculatorOption opt)
        {
            foreach(ITaxCalculatorStrategy strategy in _strategy)
            {
                if(strategy.IsApplicable(opt))
                {
                    return strategy;
                }
            }

            throw new ArgumentNullException();
        }

    }
}
