using FluentValidation;
using BeHealthy.Application.Dtos.Visit;

namespace BeHealthy.Application.Validations.Visit;

public class VisitUpdateDtoValidator : AbstractValidator<VisitUpdateDto>
{
    public VisitUpdateDtoValidator()
    {
        RuleFor(x => x.VisitDate)
            .NotEmpty().WithMessage("Visit date is required.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(256);

        RuleFor(x => x.PatientId)
            .GreaterThan(0).WithMessage("Patient is required.");

        RuleFor(x => x.DoctorId)
            .GreaterThan(0).WithMessage("Doctor is required.");

        RuleFor(x => x.MedicalRecordId)
            .GreaterThan(0).WithMessage("Medical record is required.");
    }
}