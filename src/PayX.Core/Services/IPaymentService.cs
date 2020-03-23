using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayX.Core.Models;

namespace PayX.Core.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllUserPaymentsAsync(Guid userId);

        Task<Payment> GetUserPaymentByIdAsync(Guid paymentId, Guid userId);

        Task<Payment> ProcessPaymentAsync(Payment newPayment);
    }
}