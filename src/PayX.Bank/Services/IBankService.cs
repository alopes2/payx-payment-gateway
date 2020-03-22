using System.Threading.Tasks;
using PayX.Bank.Models;

namespace PayX.Bank.Services
{
    public interface IBankService
    {
        Task<BankPaymentResult> ProcessPayment(BankPayment payment);
    }
}
