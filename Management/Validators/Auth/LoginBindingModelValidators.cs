using FluentValidation;
using Presentation.Models;

namespace Presentation.Validators
{
    public class LoginBindingModelValidator : AbstractValidator<LoginBindingModel>
    {
        public LoginBindingModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(20);
        }
    }

    public class ExternalLoginBindingModelValidator : AbstractValidator<ExternalLoginBindingModel>
    {
        public ExternalLoginBindingModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LoginProvider).NotEmpty();
            RuleFor(x => x.ProviderKey).NotEmpty();
        }
    }
}
