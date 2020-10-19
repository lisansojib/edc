using FluentValidation;
using Presentation.Participant.Models;

namespace Presentation.Participant.Validators
{
    public class PendingSpeakerBindingModelValidator : AbstractValidator<PendingSpeakerBindingModel>
    {
        public PendingSpeakerBindingModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        }
    }
}
