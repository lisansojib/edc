using FluentValidation;
using Presentation.Admin.Models.Home;
using System;

namespace Presentation.Admin.Validators.Home
{
    public class GuestBindingModelValidator : AbstractValidator<GuestBindingModel>
    {
        public GuestBindingModelValidator()
        {
            RuleFor(x => x.Title).MaximumLength(100);
            RuleFor(x => x.FirstName).MaximumLength(100);
            RuleFor(x => x.LastName).MaximumLength(100);
            RuleFor(x => x.EmailCorp).EmailAddress().When(x => x.EmailCorp.NotNullOrEmpty());
            RuleFor(x => x.EmailPersonal).EmailAddress().When(x => x.EmailCorp.NotNullOrEmpty());
            RuleFor(x => new { x.EmailCorp, x.EmailPersonal })
                .Must(x => x.EmailPersonal.NotNullOrEmpty() || x.EmailCorp.NotNullOrEmpty())
                .WithMessage("You must enter either one of 'EmailPersonal' or 'EmailCorp'");
            RuleFor(x => x.PhoneCorp).MaximumLength(20);
            RuleFor(x => x.LinkedinUrl).MaximumLength(250);
        }
    }
}
