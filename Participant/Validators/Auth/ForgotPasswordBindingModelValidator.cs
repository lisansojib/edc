using FluentValidation;
using Presentation.Participant.Models;

namespace Presentation.Participant.Validators
{
    public class ForgotPasswordBindingModelValidator : AbstractValidator<ForgotPasswordBindingModel>
    {
        public ForgotPasswordBindingModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
