using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Model;
using TaxCalculation.Core.SubModel;

namespace TaxCalculation.UnitTests
{
    public class TestBase
    {
        public Location CreateLocationData()
        {
            return new Location()
            {
                Country = "US",
                Zip = "05495-2086",
                State = "VT",
                City = "Williston",
                Street = "312 Hurricane Lane",
                TaxCalculatorOption = "tax_jar"
            };
        }

        public TaxRate CreateTaxRate()
        {
            var taxRate = new TaxRate();

            taxRate.Rate = new RateDetails { Zip = "05495-2086", Country = "US", CountryRate = 0.0F, State = "VT", StateRate = 0.06F, County = "CHITTENDEN", CountyRate = 0.0F, City = "WILLISTON", CityRate = 0.0F, CombinedDistrictRate = 0.01F, CombinedRate = 0.07F, FreightTaxable = true };

            return taxRate;
        }

        public Order CreateOrderData()
        {
            var nexusAddresses = new List<NexusAddresses>();
            nexusAddresses.Add(new NexusAddresses { Id = "Location", Country = "US", Zip = "92093", State = "CA", City = "La Jolla", Street = "9500 Gilman Drive" });

            var lineItems = new List<TaxCalculation.Core.Model.LineItems>();
            lineItems.Add(new Core.Model.LineItems { Id = "1", Quantity = 1, ProductTaxCode = "20010", UnitPrice = 15, Discount = 0 });

            return new Order { FromCountry = "US", FromZip = "92093", FromState = "CA", FromCity = "La Jolla", FromStreet = "9500 Gilman Drive", ToCountry = "US", ToZip = "90002", ToState = "CA", ToCity = "Los Angeles", ToStreet = "1335 E 103rd St", Amount = 15, Shipping = 1.5f, NexusAddresses = nexusAddresses, LineItems = lineItems, TaxCalculatorOption = "tax_jar" };
        }

        public Tax CreateTax()
        {
            var lineItem = new TaxCalculation.Core.SubModel.LineItems { Id = "1", TaxableAmount = 15f, TaxCollectable = 1.35f, CombinedTaxRate = 0.09f, StateTaxableAmount = 15f, StateSalesTaxRate = 0.0625f, StateAmount = 0.94f, CountyTaxableAmount = 15, CountyTaxRate = 0.0025f, CountyAmount = 0, CityTaxRate = 0, CityAmount = 0, SpecialDistrictTaxableAmount = 15, SpecialTaxRate = 0.025f, SpecialDistrictAmount = 0.38f };

            var lineItems = new List<TaxCalculation.Core.SubModel.LineItems>();
            lineItems.Add(lineItem);

            var breakdown = new Breakdown { TaxableAmount = 15, TaxCollectable = 1.35f, CombinedTaxRate = 0.09f, StateTaxableAmount = 15, StateTaxRate = 0.0625f, StateTaxCollectable = 0.94f, CountyTaxableAmount = 15, CountyTaxRate = 0.0025f, CountyTaxCollectable = 0.04f, CityTaxableAmount = 0, CityTaxRate = 0, CityTaxCollectable = 0, SpecialDistrictTaxableAmount = 15, SpecialTaxRate = 0.025f, SpecialDistrictTaxCollectable = 0.38f, LineItems = lineItems };

            var jurisdiction = new Jurisdictions { Country = "US", State = "CA", County = "LOS ANGELES", City = "LOS ANGELES" };

            var taxDetails = new TaxDetails { OrderTotalAmount = 16.5f, Shipping = 1.5f, TaxableAmount = 15f, AmountToCollect = 1.35f, Rate = 0.09f, HasNexus = true, FreightTaxable = false, TaxSource = "destination", Breakdown = breakdown, Jurisdictions = jurisdiction };

            return new Tax { Taxes = taxDetails };
        }
    }
}
