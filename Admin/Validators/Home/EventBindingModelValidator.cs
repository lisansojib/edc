using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Admin.Validators
{
    public class EventBindingModelValidator : AbstractValidator<EventBindingModel>
    {
        public EventBindingModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(1000);
            RuleFor(x => x.Date).NotEmpty();
        }
    }
}
