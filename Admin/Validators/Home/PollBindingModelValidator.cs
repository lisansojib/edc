using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Admin.Validators
{
    public class PollBindingModelValidator : AbstractValidator<PollBindingModel>
    {
        public PollBindingModelValidator()
        {
            RuleFor(x => x.GraphType).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Name).MaximumLength(100);
            RuleFor(x => x.Panel).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Origin).MaximumLength(200);
            RuleFor(x => x.PollDate).NotEmpty();
        }
    }
}
