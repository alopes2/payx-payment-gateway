using System;

namespace PayX.Core.Models
{
    public class Payment
    {
        public Guid Id { get; set; }

        public string CardNumber { get; set; }

        public int Cvv { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public decimal Amount { get; set; }

        public Guid CurrencyId { get; set; }
        
        public Currency Currency { get; set; }

        public bool IsSuccessful { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
