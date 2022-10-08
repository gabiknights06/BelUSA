using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculation.Core.Model;
using TaxCalculation.Services.Interfaces;

namespace TaxCalculation.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/tax-calculator")]
    [ApiController]
    public class TaxCalculatorController : ControllerBase
    {

        ITaxService _service;

        public TaxCalculatorController(ITaxService service)
        {
            _service = service;
        }


        /// <summary>
        /// Calculate the taxes for an order.
        /// </summary>
        /// <param name="order">Details of the order.</param>
        /// <returns></returns>
        [HttpPost("calc-option/{calcOption}/calculate")]
        public async Task<IActionResult> CalculateTaxes([FromBody] Order order, [FromRoute] string calcOption)
        {
            order.TaxCalculatorOption = calcOption;

            var result = await _service.CalculateTax(order);

            return Ok(result);
        }


        /// <summary>
        /// Get the tax rates for a location by zip. Conditional filter - country, Optional filter - state, city, street.
        /// </summary>
        /// <returns></returns>
        [HttpGet("calc-option/{calcOption}/tax-rate/{zip}")]
        public async Task<IActionResult> GetTaxRate([FromRoute] string calcOption, [FromRoute] string zip, [FromQuery] string? country, [FromQuery] string? state, [FromQuery] string? city, [FromQuery] string? street)
        {
            var location = new Location { Zip = zip, Country = country, State = state, City = city, Street = street, TaxCalculatorOption = calcOption };

            var result = await _service.GetTaxRateByLocation(location);

            return Ok(result);
        }

    }
}
