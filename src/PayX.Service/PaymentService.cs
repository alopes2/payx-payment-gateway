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

        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            var payments = await _unitOfWork.Payments
                .GetAllWithCurrentyAsync();

            return payments;
        }

        public async Task<Payment> GetPaymentById(Guid paymentId)
        {
            var payment = await _unitOfWork.Payments
                .GetWithCurrentyByIdAsync(paymentId);

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