using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Admin.Validators
{
    public class ShareEventLinkBindingModelValidator : AbstractValidator<ShareEventLinkBindingModel>
    {
        public ShareEventLinkBindingModelValidator()
        {
            //RuleFor(x => x.GuestName).NotEmpty();
            RuleFor(x => x.GuestEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.EventId).NotEmpty();
            RuleFor(x => x.EventTitle).NotEmpty();
        }
    }
}
