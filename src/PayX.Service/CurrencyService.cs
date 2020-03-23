using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayX.Core;
using PayX.Core.Exceptions;
using PayX.Core.Models;
using PayX.Core.Services;

namespace PayX.Service
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrenciesAsync()
        {
            var currencies = await _unitOfWork.Currencies.GetAllAsync();

            return currencies;
        }

        public async Task<Currency> CreateCurrencyAsync(string currencyName)
        {
            var normalizedName = currencyName.ToUpper();
            var existingCurrency = await _unitOfWork.Currencies.SingleOrDefaultAsync(c => c.Name.Equals(normalizedName));
            if (existingCurrency != null)
            {
                throw new HttpResponseException("Currency with this name already exists.")
                    .BadRequest();
            }

            var currency = new Currency
            {
                Name = normalizedName
            };
            
            await _unitOfWork.Currencies.AddAsync(currency);
            await _unitOfWork.CommitAsync();
            
            return currency;
        }
    }
}