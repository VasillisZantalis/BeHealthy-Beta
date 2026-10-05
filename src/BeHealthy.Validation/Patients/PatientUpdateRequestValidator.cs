using BeHealthy.Shared.Dtos.Patient;

namespace BeHealthy.Validation.Patients;

public class PatientUpdateRequestValidator : AbstractValidator<PatientUpdateRequest>
{
    public PatientUpdateRequestValidator()
    {
        RuleFor(x => x.Id).EntityId();
        RuleFor(x => x.FirstName).PersonName(Resource.FirstName);
        RuleFor(x => x.LastName).PersonName(Resource.LastName);
        RuleFor(x => x.PhoneNumber).PhoneNumber();
        RuleFor(x => x.Image).Image();
        RuleFor(x => x.DepartmentId).OptionalReference(Resource.Department);
        RuleFor(x => x.Gender).OptionalText(Resource.Gender, FieldLengths.Gender);
        RuleFor(x => x.Address).OptionalText(Resource.Address, FieldLengths.Address);
        RuleFor(x => x.DateOfBirth).NotInTheFuture(Resource.DateOfBirth);
    }
}
