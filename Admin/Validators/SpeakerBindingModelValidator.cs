using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Admin.Validators
{
    public class SpeakerBindingModelValidator : AbstractValidator<SpeakerBindingModel>
    {
        public SpeakerBindingModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Title).MaximumLength(100);
            //RuleFor(x => x.LinkedInUrl).Url();
        }
    }
}
