using System;

namespace PayX.Api.Controllers.Resources
{
    public class ProcessPaymentResource
    {
        public string CardNumber { get; set; }

        public int Cvv { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public decimal Amount { get; set; }

        public int CurrencyId { get; set; }
    }
}