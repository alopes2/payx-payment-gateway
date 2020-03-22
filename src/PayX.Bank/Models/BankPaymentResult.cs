using System;

namespace PayX.Bank.Models
{
    public class BankPaymentResult
    {
        public Guid Id { get; set; }

        public BankPaymentStatus Status { get; set; }
    }
}