using BeHealthy.Shared.Dtos.Visit;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validations.Visit;

public class VisitUpdateDtoValidator : AbstractValidator<VisitUpdateDto>
{
    public VisitUpdateDtoValidator()
    {
        RuleFor(x => x.VisitDate)
            .NotEmpty()
            .WithMessage(string.Format(Resource.PropertyRequired, Resource.VisitDate));

        RuleFor(x => x.Reason)
            .NotEmpty()
                .WithMessage(string.Format(Resource.PropertyRequired, Resource.Reason))
            .MaximumLength(256);

        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage(string.Format(Resource.PropertyRequired, Resource.Patient));

        RuleFor(x => x.DoctorId)
            .GreaterThan(0)
            .WithMessage(string.Format(Resource.PropertyRequired, Resource.Doctor));

        RuleFor(x => x.MedicalRecordId)
            .GreaterThan(0)
            .WithMessage(string.Format(Resource.PropertyRequired, Resource.MedicalRecord));
    }
}