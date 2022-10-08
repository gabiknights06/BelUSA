using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculation.Core.Model
{
    public class TaxRate
    {
        [JsonProperty(PropertyName = "rate")]
        public RateDetails Rate { get; set; }
    }
}
