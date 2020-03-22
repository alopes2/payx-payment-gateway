using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PayX.Api.Controllers.Resources;
using PayX.Core.Models;

namespace PayX.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrenciesController : ControllerBase
    {
        private static Currency[] Currencies = new[]
        {
            new Currency
            {
                Id = new Guid("c48ab9b3-1f23-461a-a993-c6c040eb7d6a"),
                Name = "EUR"
            },
            new Currency 
            {
                Id = new Guid("c48ab9b3-1f23-461a-a993-c6c040eb7d6b"),
                Name = "USD"
            }
        };
        
        // GET: currenies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyResource>>> GetCurrencies()
        {
            var currencyResources = Currencies.Select(c => new CurrencyResource
            {
                Id = c.Id,
                Name = c.Name
            });

            return Ok(currencyResources);
        }
    }
}