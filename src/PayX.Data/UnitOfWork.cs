using System.Threading.Tasks;
using PayX.Core;
using PayX.Core.Repositories;
using PayX.Data.Repositories;

namespace PayX.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PayXDbContext _context;

        private PaymentRepository _paymentRepository;

        private CurrencyRepository _currencyRepository;

        private UserRepository _userRepository;

        public UnitOfWork(PayXDbContext context)
        {
            _context = context;
        }

        public IPaymentRepository Payments => _paymentRepository = _paymentRepository ?? new PaymentRepository(_context);

        public ICurrencyRepository Currencies => _currencyRepository = _currencyRepository ?? new CurrencyRepository(_context);

        public IUserRepository Users => _userRepository = _userRepository ?? new UserRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}