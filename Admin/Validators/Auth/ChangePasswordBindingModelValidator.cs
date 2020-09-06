using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Validators
{
    public class ChangePasswordBindingModelValidator : AbstractValidator<ChangePasswordBindingModel>
    {
        public ChangePasswordBindingModelValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).MaximumLength(20);
            RuleFor(x => x.ConfirmNewPassword).Matches(x => x.NewPassword);
        }
    }
}
