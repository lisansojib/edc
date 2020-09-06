using FluentValidation;
using Presentation.Models;

namespace Presentation.Validators
{
    public class ForgotPasswordBindingModelValidator : AbstractValidator<ForgotPasswordBindingModel>
    {
        public ForgotPasswordBindingModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
