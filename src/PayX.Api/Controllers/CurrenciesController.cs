using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PayX.Api.Controllers.Resources;
using PayX.Core.Models;
using PayX.Core.Services;

namespace PayX.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyService _service;
        public CurrenciesController(ICurrencyService service)
        {
            _service = service;
        }

        // GET: currenies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyResource>>> GetCurrencies()
        {
            var currencies = await _service.GetAllCurrencies();

            var currencyResources = currencies.Select(c => new CurrencyResource
            {
                Id = c.Id,
                Name = c.Name
            });

            return Ok(currencyResources);
        }
    }
}