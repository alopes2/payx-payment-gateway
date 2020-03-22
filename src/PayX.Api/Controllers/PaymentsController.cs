using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayX.Api.Controllers.Resources;
using PayX.Core.Models;
using PayX.Core.Extensions;

namespace PayX.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private static Currency[] Currencies = new[]
        {
            new Currency
            {
                Id = new Guid("c48ab9b3-1f23-461a-a993-c6c040eb7d6a"),
                Name = "EUR"
            },
            new Currency 
            {
                Id = new Guid("c48ab9b3-1f23-461a-a993-c6c040eb7d6b"),
                Name = "USD"
            }
        };
        
        private static List<Payment> Payments = new List<Payment>()
        {
            new Payment
            {
                Id = new Guid("c48ab9b3-1f23-461a-a993-c6c040eb7d6e"),
                Amount = 100.00m,
                CardNumber = "1234 1234 1234 1234",
                Currency = Currencies[0],
                Cvv = 123,
                ExpirationMonth = 12,
                ExpirationYear = 2023,
                IsSuccessful = true,
                CreatedAt = DateTime.Now
            },
            new Payment
            {
                Id = new Guid("2b2515f8-6f65-49e4-bc13-a90c6765817d"),
                Amount = 123.12m,
                CardNumber = "1234 1234 1234 1234",
                Currency = Currencies[0],
                Cvv = 123,
                ExpirationMonth = 12,
                ExpirationYear = 2023,
                IsSuccessful = false,
                CreatedAt = DateTime.Now
            },
            new Payment
            {
                Id = new Guid("5722b9dd-f947-44cb-a361-48f5b6e323a0"),
                Amount = 222.22m,
                CardNumber = "1234 3214 1234 4444",
                Currency = Currencies[1],
                Cvv = 123,
                ExpirationMonth = 12,
                ExpirationYear = 2025,
                IsSuccessful = true,
                CreatedAt = DateTime.Now
            },
            new Payment
            {
                Id = new Guid("9e505169-05a8-4d50-afc0-b7fd726d4ddd"),
                Amount = 444.22m,
                CardNumber = "1234 3214 1234 4444",
                Currency = Currencies[1],
                Cvv = 123,
                ExpirationMonth = 12,
                ExpirationYear = 2025,
                IsSuccessful = true,
                CreatedAt = DateTime.Now
            },
        };

        public PaymentsController()
        { }

        // GET: payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentResource>>> Get()
        {
            var paymentResources = await Task.FromResult(
                Payments.Select(p => new PaymentResource
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    CardNumber = p.CardNumber.GetMaskedCardNumber(),
                    Currency = p.Currency.Name,
                    Cvv = p.Cvv,
                    ExpirationMonth = p.ExpirationMonth,
                    ExpirationYear = p.ExpirationYear,
                    IsSuccessful = p.IsSuccessful,
                    CreatedAt = p.CreatedAt
                })
            );

            return Ok(paymentResources);
        }

        // GET: payments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResource>> GetPaymentById([FromRoute] Guid id)
        {
            var payment = await Task.FromResult(
                Payments
                    .SingleOrDefault(p => p.Id.Equals(id))
            );

            var paymentResource = new PaymentResource
            {
                Id = payment.Id,
                Amount = payment.Amount,
                CardNumber = payment.CardNumber.GetMaskedCardNumber(),
                Currency = payment.Currency.Name,
                Cvv = payment.Cvv,
                ExpirationMonth = payment.ExpirationMonth,
                ExpirationYear = payment.ExpirationYear,
                IsSuccessful = payment.IsSuccessful,
                CreatedAt = payment.CreatedAt
            };

            return Ok(paymentResource);
        }

        // POST: payments
        [HttpPost]
        public async Task<ActionResult<PaymentResource>> ProcessPayment([FromBody] ProcessPaymentResource savePaymentResource)
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                Amount = savePaymentResource.Amount,
                CardNumber = savePaymentResource.CardNumber.GetMaskedCardNumber(),
                Currency = Currencies.SingleOrDefault(c => c.Id.Equals(savePaymentResource.CurrencyId)),
                Cvv = savePaymentResource.Cvv,
                ExpirationMonth = savePaymentResource.ExpirationMonth,
                ExpirationYear = savePaymentResource.ExpirationYear,
                IsSuccessful = true,
                CreatedAt = DateTime.Now
            };

            Payments.Add(payment);

            var paymentResource = new PaymentResource
            {
                Id = payment.Id,
                Amount = payment.Amount,
                CardNumber = payment.CardNumber.GetMaskedCardNumber(),
                Currency = payment.Currency.Name,
                Cvv = payment.Cvv,
                ExpirationMonth = payment.ExpirationMonth,
                ExpirationYear = payment.ExpirationYear,
                IsSuccessful = payment.IsSuccessful,
                CreatedAt = payment.CreatedAt
            };

            return Ok(paymentResource);
        }
    }
}
