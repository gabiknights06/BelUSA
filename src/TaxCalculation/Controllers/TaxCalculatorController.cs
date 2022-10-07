using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculation.Core.Model;

namespace TaxCalculation.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/tax-calculator")]
    [ApiController]
    public class TaxCalculatorController : ControllerBase
    {
        /// <summary>
        /// Calculate the taxes for an order.
        /// </summary>

        /// <returns></returns>
        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateTaxes([FromBody] Order order)
        {
            

            return Ok();
        }


        /// <summary>
        /// Get the Tax rates for a location by zip. Conditional filter - country, Optional filter - state, city, street.
        /// </summary>

        /// <returns></returns>
        [HttpGet("tax-rate/{zip}")]
        public async Task<IActionResult> GetTaxRate([FromRoute] string zip, [FromQuery] string? country, [FromQuery] string? state, [FromQuery] string? city, [FromQuery] string? street)
        {


            return Ok();
        }

    }
}
