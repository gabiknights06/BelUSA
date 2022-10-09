using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculation.Core.Model;
using TaxCalculation.Core.SubModel;

namespace TaxCalculation.Utilities
{
    public class SwaggerRequestExampleValue
    {
        public class OrderExample : IExamplesProvider<Order>
        {
            public Order GetExamples()
            {
                var nexusAddress = new List<NexusAddresses>();
                nexusAddress.Add(new NexusAddresses { Id = "Main Location", Country = "US", Zip = "92093", State = "CA", City = "La Jolla", Street = "9500 Gilman Drive" });

                var lineItems = new List<Core.Model.LineItems>();
                lineItems.Add(new Core.Model.LineItems { Id = "1", Quantity = 1, ProductTaxCode = "20010", UnitPrice = 15f, Discount = 0f});


                return new Order { 
                    FromCountry = "US",
                    FromZip = "92093",
                    FromState = "CA",
                    FromCity = "La Jolla",
                    FromStreet = "9500 Gilman Drive",
                    ToCountry = "US",
                    ToZip = "90002",
                    ToState = "CA",
                    ToCity = "Los Angeles",
                    ToStreet = "1335 E 103rd St",
                    Amount = 15f,
                    Shipping = 1.5f,
                    NexusAddresses = nexusAddress,
                    LineItems = lineItems
                };
            }
        }

        public class TaxResponseExample : IExamplesProvider<Tax>
        {
            public Tax GetExamples()
            {
                var lineItems = new List<Core.SubModel.LineItems>();
                lineItems.Add(new Core.SubModel.LineItems { Id = "1", TaxableAmount = 15f, TaxCollectable = 1.43f, CombinedTaxRate = 0.095f, StateTaxableAmount = 15f, StateSalesTaxRate = 0.0625f, StateAmount = 0.94f, CountyTaxableAmount = 15f, CountyTaxRate = 0.01f, CountyAmount = 0.15f, CityTaxableAmount = 0f, CityTaxRate = 0.01f, CityAmount = 0f, SpecialDistrictTaxableAmount = 0.34f, SpecialTaxRate = 0.0225f, SpecialDistrictAmount = 0.34f,  });

                var breakdown = new Breakdown { TaxableAmount = 0, TaxCollectable = 0, CombinedTaxRate = 0, StateTaxableAmount = 0, StateTaxRate = 0, StateTaxCollectable = 0, CountyTaxableAmount = 0, CountyTaxRate = 0, CountyTaxCollectable = 0f, CityTaxableAmount = 0f, CityTaxRate = 0f, CityTaxCollectable = 0, SpecialDistrictTaxableAmount = 0f, SpecialTaxRate = 0f, SpecialDistrictTaxCollectable = 0f, LineItems = lineItems};

                var jurisdictions = new Jurisdictions { Country = "US", State = "LOS ANGELES", County = "LOS ANGELES COUNTY", City = "LOS ANGELES" };

                var taxDetails = new TaxDetails { OrderTotalAmount = 16.5f, Shipping = 1.5f, TaxableAmount = 15f, AmountToCollect = 1.43f, Rate = 0.095f, HasNexus = true, FreightTaxable = false, TaxSource = "destination", Jurisdictions = jurisdictions, Breakdown = breakdown};

                return new Tax {
                    Taxes = taxDetails
                };
            }
        }

    }
}
