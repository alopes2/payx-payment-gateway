using System;

namespace PayX.Api.Controllers.Resources
{
    public class PaymentResource
    {
        public Guid Id { get; set; }

        public string CardNumber { get; set; }

        public int Cvv { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public bool IsSuccessful { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}