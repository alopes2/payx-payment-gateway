using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayX.Core.Models;

namespace PayX.Core.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPayments();

        Task<Payment> GetPaymentById(Guid paymentId);

        Task<Payment> ProcessPayment(Payment newPayment);
    }
}