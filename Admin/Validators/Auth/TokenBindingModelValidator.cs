using FluentValidation;
using Presentation.Admin.Models;
using System;

namespace Presentation.Validators
{
    public class TokenBindingModelValidator : AbstractValidator<TokenBindingModel>
    {
        public TokenBindingModelValidator()
        {
            RuleFor(x => x.AccessToken).EmailAddress();
            RuleFor(x => x.ExpiresAtUtc).NotEmpty().Must(x => x > DateTime.UtcNow);
        }
    }
}
