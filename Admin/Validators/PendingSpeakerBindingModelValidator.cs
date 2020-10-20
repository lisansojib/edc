using FluentValidation;
using Presentation.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Admin.Validators
{
    public class PendingSpeakerBindingModelValidator : AbstractValidator<PendingSpeakerBindingModel>
    {
        public PendingSpeakerBindingModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        }
    }
}
