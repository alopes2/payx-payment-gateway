using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayX.Core;
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

        public async Task<IEnumerable<Currency>> GetAllCurrencies()
        {
            var currencies = await _unitOfWork.Currencies.GetAllAsync();

            return currencies;
        }
    }
}