using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Model;

namespace TaxCalculation.Services.Interfaces
{
    public interface ITaxService
    {
        Task<Rate> GetTaxRateByLocation(Location data);
        Task<Tax> CalculateTax(Order data);
    }
}
