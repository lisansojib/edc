using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Admin.Validators
{
    public class ShareEventLinkBindingModelValidator : AbstractValidator<ShareEventLinkBindingModel>
    {
        public ShareEventLinkBindingModelValidator()
        {
            RuleFor(x => x.GuestEmails).NotEmpty();
            RuleForEach(x => x.GuestEmails).NotEmpty().EmailAddress();
            RuleFor(x => x.EventId).NotEmpty();
            RuleFor(x => x.EventTitle).NotEmpty();
        }
    }
}
