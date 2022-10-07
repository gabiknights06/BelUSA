using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Enumeration;
using TaxCalculation.Core.SubModel;

namespace TaxCalculation.Core.Model
{
    public class Order
    {
        public string FromCountry { get; set; }
        public string FromZip { get; set; }
        public string FromState { get; set; }
        public string FromCity { get; set; }
        public string FromStreet { get; set; }
        public string ToCountry { get; set; }
        public string ToZip { get; set; }
        public string ToState { get; set; }
        public string ToCity { get; set; }
        public string ToStreet { get; set; }
        public float Amount { get; set; }
        public float Shipping { get; set; }
        public List<NexusAddresses> NexusAddresses { get; set; }
        public List<LineItems> LineItems { get; set; }
        public TaxCalculatorOption TaxCalculatorOption { get; set; }
    }

    public class LineItems
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public string ProductTaxCode { get; set; }
        public float UnitPrice { get; set; }
        public float Discount { get; set; }
    }
}
