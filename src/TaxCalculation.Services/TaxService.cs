using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Model;
using TaxCalculation.Services.Interfaces;

namespace TaxCalculation.Services
{
    public class TaxService : ITaxService
    {
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
