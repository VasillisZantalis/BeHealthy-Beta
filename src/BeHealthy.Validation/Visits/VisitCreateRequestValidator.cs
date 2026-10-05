using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.Validation.Visits;

public class VisitCreateRequestValidator : AbstractValidator<VisitCreateRequest>
{
    public VisitCreateRequestValidator()
    {
        RuleFor(x => x.VisitDate)
            .NotEmpty().WithMessage(ValidationMessages.Required(Resource.VisitDate));

        RuleFor(x => x.Reason).RequiredText(Resource.Reason, FieldLengths.VisitReason);
        RuleFor(x => x.Notes).OptionalText(Resource.Notes, FieldLengths.VisitNotes);
        RuleFor(x => x.PatientId).RequiredReference(Resource.Patient);
        RuleFor(x => x.DoctorId).RequiredReference(Resource.Doctor);
        RuleFor(x => x.MedicalRecordId).RequiredReference(Resource.MedicalRecord);
    }
}
