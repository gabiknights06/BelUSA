using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Enumeration;
using TaxCalculation.Core.Model;
using TaxCalculation.Core.Strategy;
using TaxCalculation.Services.Extensions;
using TaxCalculation.Services.Interfaces;

namespace TaxCalculation.Services
{
    public class TaxService : ITaxService
    {
        ITaxCalculatorStrategyFactory _strategyFactory;
        public TaxService(ITaxCalculatorStrategyFactory strategyFactory)
        {
            _strategyFactory = strategyFactory;
        }


        public async Task<Tax> CalculateTax(Order data)
        {
            throw new NotImplementedException();
        }

        public async Task<TaxRate> GetTaxRateByLocation(Location data)
        {
            var factory = _strategyFactory.Create(data.TaxCalculatorOption.ToEnum<TaxCalculatorOption>());

           return await factory.GetTaxRateByLocation(data);
        }
    }
}
