using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using TaxCalculation.Core.Enumeration;
using TaxCalculation.Core.Model;
using TaxCalculation.Core.Strategy;
using TaxCalculation.Persistent.Exceptions;
using TaxCalculation.Services;
using Xunit;

namespace TaxCalculation.UnitTests
{
    public class TaxServiceTest : TestBase
    {
        [Fact]
        public async void GetTaxRateByLocation_InputValidData_True()
        {
            var expected = CreateTaxRate();

            var calculatorMock = new Mock<ITaxCalculatorStrategy>();
            calculatorMock.Setup(x => x.GetTaxRateByLocation(It.IsAny<Location>())).Returns(Task.FromResult(CreateTaxRate()));

            var taxStrategyFactoryMock = new Mock<ITaxCalculatorStrategyFactory>();
            taxStrategyFactoryMock.Setup(x => x.Create(It.IsAny<TaxCalculatorOption>())).Returns(calculatorMock.Object);

            var serviceMock = new Mock<TaxService>(taxStrategyFactoryMock.Object) { CallBase = true };

            var actual = await serviceMock.Object.GetTaxRateByLocation(CreateLocationData());

            expected.Should().BeEquivalentTo(actual);
        }

        [Fact]
        public void GetTaxRateByLocation_NoTaxCalculatorSelected_ThrowBadRequestException()
        {
            var taxStrategyFactoryMock = new Mock<ITaxCalculatorStrategyFactory>();
            taxStrategyFactoryMock.Setup(x => x.Create(It.IsAny<TaxCalculatorOption>()));

            var serviceMock = new Mock<TaxService>(taxStrategyFactoryMock.Object) { CallBase = true };

            Func<Task<TaxRate>> actual = () => serviceMock.Object.GetTaxRateByLocation(CreateLocationData());

            actual.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async void CalculateTax_InputValidData_True()
        {
            var expected = CreateTax();

            var calculatorMock = new Mock<ITaxCalculatorStrategy>();
            calculatorMock.Setup(x => x.CalculateTax(It.IsAny<Order>())).Returns(Task.FromResult(CreateTax()));

            var taxStrategyFactoryMock = new Mock<ITaxCalculatorStrategyFactory>();
            taxStrategyFactoryMock.Setup(x => x.Create(It.IsAny<TaxCalculatorOption>())).Returns(calculatorMock.Object);

            var serviceMock = new Mock<TaxService>(taxStrategyFactoryMock.Object) { CallBase = true };

            var actual = await serviceMock.Object.CalculateTax(CreateOrderData());

            expected.Should().BeEquivalentTo(actual);
        }

        [Fact]
        public void CalculateTax_NoTaxCalculatorSelected_ThrowBadRequestException()
        {
            var taxStrategyFactoryMock = new Mock<ITaxCalculatorStrategyFactory>();
            taxStrategyFactoryMock.Setup(x => x.Create(It.IsAny<TaxCalculatorOption>()));

            var serviceMock = new Mock<TaxService>(taxStrategyFactoryMock.Object) { CallBase = true };

            Func<Task<Tax>> actual = () => serviceMock.Object.CalculateTax(CreateOrderData());

            actual.Should().ThrowAsync<BadRequestException>();
        }

    }
}
