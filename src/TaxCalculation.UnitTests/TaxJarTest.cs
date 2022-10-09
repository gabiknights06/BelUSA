using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaxCalculation.Core.Model;
using TaxCalculation.Core.Strategy;
using Xunit;

namespace TaxCalculation.UnitTests
{
    public class TaxJarTest : TestBase
    {
        [Fact]
        public async void GetTaxRateByLocation_InputValidData_True()
        {            
            var expected = CreateTaxRate();

            var calculatorMock = new Mock<ITaxCalculatorStrategy>();
            calculatorMock.Setup(x => x.GetTaxRateByLocation(It.IsAny<Location>())).Returns(Task.FromResult(CreateTaxRate()));

            var actual = await calculatorMock.Object.GetTaxRateByLocation(CreateLocationData());

            expected.Should().BeEquivalentTo(actual); 
        }

        [Fact]
        public async void CalculateTax_InputValidData_True()
        {
            var expected = CreateTax();

            var calculatorMock = new Mock<ITaxCalculatorStrategy>();
            calculatorMock.Setup(x => x.CalculateTax(It.IsAny<Order>())).Returns(Task.FromResult(CreateTax()));

            var actual = await calculatorMock.Object.CalculateTax(CreateOrderData());

            expected.Should().BeEquivalentTo(actual);
        }



    }
}
