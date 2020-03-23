using FluentValidation;
using PayX.Api.Controllers.Resources;

namespace PayX.Api.Validators
{
    public class AuthResourceValidator : AbstractValidator<AuthResource>
    {
        public AuthResourceValidator()
        {
            RuleFor(a => a.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(a => a.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty.");
        }
    }
}