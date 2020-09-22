using FluentValidation;
using Presentation.Admin.Models;

namespace Presentation.Admin.Validators
{
    public class CompanyBindingModelValidator : AbstractValidator<CompanyBindingModel>
    {
        public CompanyBindingModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).MaximumLength(500);
            RuleFor(x => x.Phone).MaximumLength(20);
            RuleFor(x => x.Website).MaximumLength(250);
        }
    }
}
