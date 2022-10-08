﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Model;

namespace TaxCalculation.Core.Repositories
{
    public interface ITaxCalculator
    {
        RateDetails GetTaxRateByLocation(Location data);


    }
}
