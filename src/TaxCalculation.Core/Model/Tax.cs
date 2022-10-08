using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculation.Core.Model
{
    public class Tax
    {
        [JsonProperty(PropertyName = "tax")]
        public TaxDetails Taxes { get; set; }
    }
}
