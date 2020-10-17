using FluentValidation;
using Presentation.Participant.Models;

namespace Presentation.Participant.Validators
{
    public class PendingSepakerBindingModelValidator : AbstractValidator<PendingSepakerBindingModel>
    {
        public PendingSepakerBindingModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        }
    }
}
