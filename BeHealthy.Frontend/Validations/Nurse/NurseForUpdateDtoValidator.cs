using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Locales;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeHealthy.Frontend.Validations.Nurse;

public class NurseForUpdateDtoValidator : AbstractValidator<NurseUpdateDto>
{
    public NurseForUpdateDtoValidator()
    {
        RuleFor(x => x.FirstName)
         .NotEmpty()
         .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.FirstName));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.LastName));

        RuleFor(x => x.PhoneNumber)
          .Matches(@"^\+?[1-9]\d{1,14}$")
          .WithMessage(string.Format(Resource.PropertyInvalidFormat, Resource.PhoneNumber))
          .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}
