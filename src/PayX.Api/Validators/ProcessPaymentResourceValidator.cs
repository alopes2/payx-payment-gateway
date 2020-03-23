using System;
using System.Globalization;
using FluentValidation;
using PayX.Api.Controllers.Resources;

namespace PayX.Api.Validators
{
    public class ProcessPaymentResourceValidator : AbstractValidator<ProcessPaymentResource>
    {
        public ProcessPaymentResourceValidator()
        {
            RuleFor(p => p.CardNumber)
                .NotEmpty()
                .CreditCard()
                .WithMessage("Invalid card number.");

            RuleFor(p => p.CurrencyId)
                .NotEmpty()
                .WithMessage("Currency Id not specified.");

            RuleFor(p => p.Cvv)
                .NotEmpty()
                .Must(c => c.ToString().Length == 3)
                .WithMessage("CVV must not be empty and have 3 numbers.");

            RuleFor(p => p.ExpirationYear)
                .NotEmpty()
                .GreaterThanOrEqualTo(p => DateTime.Today.Month == 12
                    ? DateTime.Today.Year + 1
                    : DateTime.Today.Year)
                .WithMessage("Expiration Year invalid.");

            RuleFor(p => p.ExpirationMonth)
                .NotEmpty()
                .GreaterThan(p => DateTime.Today.Year == p.ExpirationYear 
                    ? DateTime.Today.Month
                    : 0)
                .LessThan(13)
                .WithMessage("Expiration Month invalid.");
            
            RuleFor(p => p.Amount)
                .Must(a => 
                {
                    var valueAfterDecimalPoint = a.ToString(CultureInfo.InvariantCulture).Split('.')[1];
                    return valueAfterDecimalPoint == null || valueAfterDecimalPoint.Length <= 2;
                })
                .WithMessage("Amount should have only two numbers after decimal point.");
        }
    }
}