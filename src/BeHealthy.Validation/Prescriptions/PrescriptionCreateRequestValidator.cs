using BeHealthy.Shared.Dtos.Prescription;

namespace BeHealthy.Validation.Prescriptions;

public class PrescriptionCreateRequestValidator : AbstractValidator<PrescriptionCreateRequest>
{
    public PrescriptionCreateRequestValidator()
    {
        RuleFor(x => x.PatientId).RequiredReference(Resource.Patient);
        RuleFor(x => x.DoctorId).RequiredReference(Resource.Doctor);
        RuleFor(x => x.Medication).RequiredText(Resource.Medication, FieldLengths.Medication);
        RuleFor(x => x.Dosage).RequiredText(Resource.Dosage, FieldLengths.Dosage);

        RuleFor(x => x.DatePrescribed)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ValidationMessages.Required(Resource.DatePrescribed))
            .NotInTheFuture(Resource.DatePrescribed);
    }
}
