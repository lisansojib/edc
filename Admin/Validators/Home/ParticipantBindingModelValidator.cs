using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Admin.Validators
{
    public class ParticipantBindingModelValidator : AbstractValidator<ParticipantBindingModel>
    {
        public ParticipantBindingModelValidator()
        {
            RuleFor(x => x.Title).MaximumLength(100);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.EmailCorp).NotEmpty().EmailAddress();
            RuleFor(x => x.EmailPersonal).NotEmpty().EmailAddress();
            RuleFor(x => x.Phone).MaximumLength(20);
            RuleFor(x => x.PhoneCorp).MaximumLength(20);
            RuleFor(x => x.LinkedinUrl).MaximumLength(250);
            RuleFor(x => x.CompanyName).MaximumLength(100);
        }
    }
}
