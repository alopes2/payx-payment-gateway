using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayX.Core.Models;

namespace PayX.Core.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    { 
        Task<IEnumerable<Payment>> GetAllWithCurrencyByUserIdAsync(Guid userId);
        
        Task<Payment> GetWithCurrencyByIdAsync(Guid id);
    }
}