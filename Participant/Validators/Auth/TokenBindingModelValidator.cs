using FluentValidation;
using Presentation.Participant.Models;
using System;

namespace Presentation.Participant.Validators
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
