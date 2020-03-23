using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayX.Core.Models;

namespace PayX.Core.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllUserPayments(Guid userId);

        Task<Payment> GetUserPaymentById(Guid paymentId, Guid userId);

        Task<Payment> ProcessPayment(Payment newPayment);
    }
}