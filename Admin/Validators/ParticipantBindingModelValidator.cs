using FluentValidation;
using Presentation.Admin.Models;
using System;

namespace Presentation.Admin.Validators
{
    public class ParticipantBindingModelValidator : AbstractValidator<ParticipantBindingModel>
    {
        public ParticipantBindingModelValidator()
        {
            RuleFor(x => x.Title).MaximumLength(100);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.EmailCorp).EmailAddress().When(x => x.EmailCorp.NotNullOrEmpty());
            RuleFor(x => x.EmailPersonal).EmailAddress().When(x => x.EmailCorp.NotNullOrEmpty());
            RuleFor(x => new { x.EmailCorp, x.EmailPersonal })
                .Must(x => x.EmailPersonal.NotNullOrEmpty() || x.EmailCorp.NotNullOrEmpty())
                .WithMessage("You must enter either one of 'EmailPersonal' or 'EmailCorp'");
            RuleFor(x => x.Phone).MaximumLength(20);
            RuleFor(x => x.PhoneCorp).MaximumLength(20);
            RuleFor(x => x.LinkedinUrl).MaximumLength(250);
            RuleFor(x => x.CompanyName).MaximumLength(100);
        }
    }
}
