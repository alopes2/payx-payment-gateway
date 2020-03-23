using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayX.Bank.Models;
using PayX.Bank.Services;
using PayX.Core;
using PayX.Core.Models;
using PayX.Core.Services;

namespace PayX.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBankService _bankSerice;

        public PaymentService(IUnitOfWork unitOfWork, IBankService bankSerice)
        {
            _bankSerice = bankSerice;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Payment>> GetAllUserPayments(Guid userId)
        {
            var payments = await _unitOfWork.Payments
                .GetAllWithCurrencyByUserIdAsync(userId);

            return payments;
        }

        public async Task<Payment> GetUserPaymentById(Guid paymentId, Guid userId)
        {
            var payment = await _unitOfWork.Payments
                .GetWithCurrencyByIdAsync(paymentId);

            if (!payment.UserId.Equals(userId))
            {
                throw new Exception();
            }

            return payment;
        }

        public async Task<Payment> ProcessPayment(Payment newPayment)
        {
            var currency = await _unitOfWork
                .Currencies
                .GetByIdAsync(newPayment.CurrencyId);

            if (currency is null)
            {
                throw new Exception($"Currency of Id {newPayment.CurrencyId} could not be found.");
            }

            var bankPayment = new BankPayment
            {
                Amount = newPayment.Amount,
                CardNumber = newPayment.CardNumber,
                Currency = currency.Name,
                Cvv = newPayment.Cvv,
                ExpirationMonth = newPayment.ExpirationMonth,
                ExpirationYear = newPayment.ExpirationYear, 
            };

            var result = await _bankSerice.ProcessPayment(bankPayment);

            newPayment.Id = result.Id;
            newPayment.IsSuccessful = result.Status == BankPaymentStatus.Succcessful;
            newPayment.CreatedAt = DateTime.Now;

            await _unitOfWork.Payments.AddAsync(newPayment);
            await _unitOfWork.CommitAsync();

            return newPayment;
        }
    }
}