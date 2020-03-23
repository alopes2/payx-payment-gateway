using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PayX.Api.Controllers;
using PayX.Api.Controllers.Resources;
using PayX.Core.Models;
using PayX.Core.Services;
using Xunit;

namespace PayX.UnitTests.Api.Controllers
{
    public class CurrenciesControllerTests
    {
        private Mock<ICurrencyService> _service;

        private Mock<IMapper> _mapper;

        private CurrenciesController _controller;

        public CurrenciesControllerTests()
        {
            _service = new Mock<ICurrencyService>();
            _mapper = new Mock<IMapper>();
            _controller = new CurrenciesController(_mapper.Object, _service.Object);
        }

        [Fact]
        public async Task GetCurrencies_NormalRequest_ReturnOk()
        {
            var response = await _controller.GetCurrenciesAsync();

            var result = response.Result as ObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task CreateCurrency_ValidCurrencyName_CreateAndReturnCurrency()
        {
            var currencyName = "testCurrency";
            var newCurrency = new Currency { Id = Guid.NewGuid(), Name = currencyName };

            _service.Setup(s => s.CreateCurrencyAsync(currencyName))
                .ReturnsAsync(newCurrency);

            _mapper.Setup(m => m.Map<Currency, CurrencyResource>(newCurrency))
                .Returns(new CurrencyResource { Id = newCurrency.Id, Name = newCurrency.Name });

            var response = await _controller.CreateCurrencyAsync(currencyName);

            var result = response.Result as ObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.IsType(typeof(CurrencyResource), result.Value);

            ResetMocks();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task CreateCurrency_InvalidCurrencyName_ReturnBadRequest(string currencyName)
        {
            var response = await _controller.CreateCurrencyAsync(currencyName);

            var result = response.Result as ObjectResult;

            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        private void ResetMocks()
        {
            _mapper.Reset();
            _service.Reset();
        }
    }
}