using System;
using System.Threading.Tasks;
using PayX.Bank.Models;

namespace PayX.Bank.Services
{
    public class BankService : IBankService
    {
        public async Task<BankPaymentResult> ProcessPayment(BankPayment payment)
        {
            var paymentResult = new BankPaymentResult
            {
                Id = Guid.NewGuid(),
                Status = BankPaymentStatus.Succcessful
            };

            MakePayment(paymentResult);

            return await Task.FromResult(paymentResult);
        }

        private static void MakePayment(BankPaymentResult paymentResult)
        {
            var randomNumber = new Random();

            var isPaymentSuccessful = randomNumber.NextDouble() >= 0.5d;
            if (!isPaymentSuccessful)
            {
                paymentResult.Status = BankPaymentStatus.Failed;
            }
        }
    }
}