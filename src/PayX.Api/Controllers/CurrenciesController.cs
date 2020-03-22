using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public CurrenciesController(IMapper mapper, ICurrencyService service)
        {
            _mapper = mapper;
            _service = service;
        }

        // GET: currenies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyResource>>> GetCurrencies()
        {
            var currencies = await _service.GetAllCurrencies();

            var currencyResources = 
                _mapper.Map<IEnumerable<Currency>, IEnumerable<CurrencyResource>>(currencies);

            return Ok(currencyResources);
        }

        // Post: currencies
        [HttpPost]
        public async Task<ActionResult<CurrencyResource>> CreateCurrency([FromBody] string currencyName)
        {
            var newCurrency = await _service.CreateCurrency(currencyName);

            var currencyResource =
                _mapper.Map<Currency, CurrencyResource>(newCurrency);

            return Ok(currencyResource);
        }
    }
}