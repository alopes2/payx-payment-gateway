using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayX.Core;
using PayX.Core.Models;
using PayX.Core.Services;

namespace PayX.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
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
            newPayment.Id = Guid.NewGuid();
            newPayment.IsSuccessful = new Random().NextDouble() >= 0.5d;
            newPayment.CreatedAt = DateTime.Now;

            await _unitOfWork.Payments.AddAsync(newPayment);
            await _unitOfWork.CommitAsync();

            return newPayment;
        }
    }
}