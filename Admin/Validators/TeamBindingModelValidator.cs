using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Admin.Validators.Home
{
    public class TeamBindingModelValidator : AbstractValidator<TeamBindingModel>
    {
        public TeamBindingModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Participants).NotEmpty();
        }
    }
}
