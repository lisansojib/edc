using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Validators
{
    public class TokenBindingModelValidator : AbstractValidator<TokenBindingModel>
    {
        public TokenBindingModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(20);
        }
    }
}
