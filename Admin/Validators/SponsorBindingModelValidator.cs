using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Admin.Validators
{
    public class SponsorBindingModelValidator : AbstractValidator<SponsorBindingModel>
    {
        public SponsorBindingModelValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ContactPerson).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ContactPersonPhone).NotEmpty().MaximumLength(20);
            RuleFor(x => x.ContactPersonEmail).EmailAddress().NotEmpty();
            RuleFor(x => x.Description).MaximumLength(1000);
            //RuleFor(x => x.Website).Url();
        }
    }
}
