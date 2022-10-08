using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculation.Core.SubModel
{
    public class Breakdown
    {
        [JsonProperty(PropertyName = "taxable_amount")]
        public float TaxableAmount { get; set; }

        [JsonProperty(PropertyName = "tax_collectable")]
        public float TaxCollectable { get; set; } 

        [JsonProperty(PropertyName = "combined_tax_rate")]
        public float CombinedTaxRate { get; set; } 

        [JsonProperty(PropertyName = "state_taxable_amount")]
        public float StateTaxableAmount { get; set; } 

        [JsonProperty(PropertyName = "state_tax_rate")]
        public float StateTaxRate { get; set; } 

        [JsonProperty(PropertyName = "state_tax_collectable")]
        public float StateTaxCollectable { get; set; } 

        [JsonProperty(PropertyName = "county_taxable_amount")]
        public float CountyTaxableAmount { get; set; } 

        [JsonProperty(PropertyName = "county_tax_rate")]
        public float CountyTaxRate { get; set; } 

        [JsonProperty(PropertyName = "county_tax_collectable")]
        public float CountyTaxCollectable { get; set; } 

        [JsonProperty(PropertyName = "city_taxable_amount")]
        public float CityTaxableAmount { get; set; } 

        [JsonProperty(PropertyName = "city_tax_rate")]
        public float CityTaxRate { get; set; } 

        [JsonProperty(PropertyName = "city_tax_collectable")]
        public float CityTaxCollectable { get; set; } 

        [JsonProperty(PropertyName = "special_district_taxable_amount")]
        public float SpecialDistrictTaxableAmount { get; set; }

        [JsonProperty(PropertyName = "special_tax_rate")]
        public float SpecialTaxRate { get; set; }

        [JsonProperty(PropertyName = "special_district_tax_collectable")]
        public float SpecialDistrictTaxCollectable { get; set; }

        [JsonProperty(PropertyName = "line_items")]
        public List<LineItems> LineItems { get; set; } 
    }
}
