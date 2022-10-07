using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculation.Core.SubModel
{
    public class Breakdown
    {
        public float TaxableAmount { get; set; } 
        public float TaxCollectable { get; set; } 
        public float CombinedTaxRate { get; set; } 
        public float StateTaxableAmount { get; set; } 
        public float StateTaxRate { get; set; } 
        public float StateTaxCollectable { get; set; } 
        public float CountyTaxableAmount { get; set; } 
        public float CountyTaxRate { get; set; } 
        public float CountyTaxCollectable { get; set; } 
        public float CityTaxableAmount { get; set; } 
        public float CityTaxRate { get; set; } 
        public float CityTaxCollectable { get; set; }
        public float SpecialDistrictTaxableAmount { get; set; }
        public float SpecialTaxRate { get; set; }
        public float SpecialDistrictTaxCollectable { get; set; }
        public List<LineItems> LineItems { get; set; }
    }
}
