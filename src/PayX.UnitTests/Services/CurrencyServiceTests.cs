using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Moq;
using PayX.Core;
using PayX.Core.Exceptions;
using PayX.Core.Models;
using PayX.Service;
using Xunit;

namespace PayX.UnitTests.Services
{
    public class CurrencyServiceTests
    {
        private CurrencyService _currencyService;

        private Mock<IUnitOfWork> _unitOfWork;
        
        public CurrencyServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _currencyService = new CurrencyService(_unitOfWork.Object);
        }
        
        [Fact]
        public async Task GetAllCurrenciesAsync_AnyRequest_ReturnIEnumerableOfCurrenciesAsync()
        {
            var currencies = new List<Currency>();
            _unitOfWork.Setup(u => u.Currencies.GetAllAsync())
                .ReturnsAsync(currencies);
                
            var result = await _currencyService.GetAllCurrenciesAsync();

            Assert.True(result is IEnumerable<Currency>);
        }
        
        [Fact]
        public async Task CreateCurrencyAsync_NonExistingCurrency_ReturnCreatedCurrency()
        {
            var currencyName = "EUR";
            Currency nonExistingCurrency = null;
            _unitOfWork.Setup(u => u.Currencies.SingleOrDefaultAsync(It.IsAny<Expression<Func<Currency, bool>>>()))
                .ReturnsAsync(nonExistingCurrency);

            var result = await _currencyService.CreateCurrencyAsync(currencyName);

            Assert.Equal(currencyName, result.Name);
        }
        
        [Fact]
        public async Task CreateCurrencyAsync_CurrencyNameLowerCase_ReturnCreatedCurrencyWithUppercaseName()
        {
            var currencyName = "eur";
            Currency nonExistingCurrency = null;
            _unitOfWork.Setup(u => u.Currencies.SingleOrDefaultAsync(It.IsAny<Expression<Func<Currency, bool>>>()))
                .ReturnsAsync(nonExistingCurrency);

            var result = await _currencyService.CreateCurrencyAsync(currencyName);

            Assert.Equal(currencyName.ToUpper(), result.Name);
        }
        
        [Fact]
        public async Task CreateCurrencyAsync_ExistingCurrency_ThrowsHttpResponseExceptionWithStatusBadRequest()
        {
            var currencyName = "eur";
            Currency nonExistingCurrency = new Currency();
            _unitOfWork.Setup(u => u.Currencies.SingleOrDefaultAsync(It.IsAny<Expression<Func<Currency, bool>>>()))
                .ReturnsAsync(nonExistingCurrency);

            var result = await Assert.ThrowsAsync<HttpResponseException>(() => _currencyService.CreateCurrencyAsync(currencyName));

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}