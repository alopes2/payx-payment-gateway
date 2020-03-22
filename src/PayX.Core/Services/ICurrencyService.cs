using System.Collections.Generic;
using System.Threading.Tasks;
using PayX.Core.Models;

namespace PayX.Core.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetAllCurrencies();

        Task<Currency> CreateCurrency(Currency currency);
    }
}