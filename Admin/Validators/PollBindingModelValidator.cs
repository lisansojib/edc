using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Admin.Validators
{
    public class PollBindingModelValidator : AbstractValidator<PollBindingModel>
    {
        public PollBindingModelValidator()
        {
            RuleFor(x => x.GraphTypeId).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(100);
            RuleFor(x => x.EventId).NotEmpty();
        }
    }
}
