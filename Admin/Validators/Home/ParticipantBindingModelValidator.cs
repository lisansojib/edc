using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Admin.Validators
{
    public class ParticipantBindingModelValidator : AbstractValidator<ParticipantBindingModel>
    {
        public ParticipantBindingModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
            RuleFor(x => x.FirstName).MaximumLength(100);
            RuleFor(x => x.LastName).MaximumLength(100);
            RuleFor(x => x.Email).EmailAddress();
            //RuleFor(x => x.Password).MaximumLength(20);
            RuleFor(x => x.Phone).MaximumLength(20);
            RuleFor(x => x.Mobile).MaximumLength(20);
            RuleFor(x => x.Title).MaximumLength(100);
            RuleFor(x => x.EmailCorp).EmailAddress();
            RuleFor(x => x.PhoneCorp).MaximumLength(20);
            RuleFor(x => x.LinkedinUrl).MaximumLength(250);
            RuleFor(x => x.CompanyId).NotEmpty();
        }
    }
}
