using FluentValidation;
using Presentation.Participant.Models;

namespace Presentation.Participant.Validators
{
    public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordBindingModel>
    {
        public ResetPasswordModelValidator()
        {
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(20);
            RuleFor(x => x.ConfirmPassword).Matches(x => x.Password);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
