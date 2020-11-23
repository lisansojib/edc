using FluentValidation;
using Presentation.Admin.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Admin.Validators.Home
{
    public class GuestBindingModelValidator: AbstractValidator<GuestBindingModel>
    {
        public GuestBindingModelValidator()
        {
            RuleFor(x => x.Title).MaximumLength(100);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.EmailCorp).NotEmpty().EmailAddress();
            RuleFor(x => x.EmailPersonal).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneCorp).MaximumLength(20);
            RuleFor(x => x.LinkedinUrl).MaximumLength(250);
            RuleFor(x => x.GuestTypeId).NotEmpty();
        }
    }
}
