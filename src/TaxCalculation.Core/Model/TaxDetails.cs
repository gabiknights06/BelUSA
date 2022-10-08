using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.SubModel;

namespace TaxCalculation.Core.Model
{
    public class TaxDetails
    {
        [JsonProperty(PropertyName = "order_total_amount")]
        public float OrderTotalAmount { get; set; }

        [JsonProperty(PropertyName = "shipping")]
        public float Shipping { get; set; }

        [JsonProperty(PropertyName = "taxable_amount")] 
        public float TaxableAmount { get; set; }

        [JsonProperty(PropertyName = "amount_to_collect")]
        public float AmountToCollect { get; set; }

        [JsonProperty(PropertyName = "rate")]
        public float Rate { get; set; }

        [JsonProperty(PropertyName = "has_nexus")]
        public bool HasNexus { get; set; }

        [JsonProperty(PropertyName = "freight_taxable")]
        public bool FreightTaxable { get; set; }

        [JsonProperty(PropertyName = "tax_source")]
        public string TaxSource { get; set; }

        [JsonProperty(PropertyName = "jurisdictions")]
        public Jurisdictions Jurisdictions { get; set; }

        [JsonProperty(PropertyName = "breakdown")]
        public Breakdown Breakdown { get; set; }
    }
}
