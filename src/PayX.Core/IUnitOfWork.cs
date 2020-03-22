using System;
using System.Threading.Tasks;
using PayX.Core.Repositories;

namespace PayX.Core
{

    public interface IUnitOfWork : IDisposable
    {
        IPaymentRepository Payments { get; }

        ICurrencyRepository Currencies { get; }
        
        Task<int> CommitAsync();
    }
}