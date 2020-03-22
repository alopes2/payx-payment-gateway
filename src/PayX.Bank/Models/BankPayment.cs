using System;

namespace PayX.Bank.Models
{
    public class BankPayment
    {
        public Guid Id { get; set; }

        public string CardNumber { get; set; }

        public int Cvv { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public bool IsSuccessful { get; set; }
    }
}