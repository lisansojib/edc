using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Admin.Validators
{
    public class AnnouncementBindingModelValidator : AbstractValidator<AnnouncementBindingModel>
    {
        public AnnouncementBindingModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(1000);
            RuleFor(x => x.CallAction).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CallAction).MaximumLength(500);
            RuleFor(x => x.Expiration).NotEmpty();
        }
    }
}
