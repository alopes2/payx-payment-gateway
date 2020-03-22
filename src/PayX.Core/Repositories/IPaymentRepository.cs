using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayX.Core.Models;

namespace PayX.Core.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    { 
        Task<IEnumerable<Payment>> GetAllWithCurrentyAsync();
        
        Task<Payment> GetWithCurrentyByIdAsync(Guid id);
    }
}