using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.Validation.Rooms;

public class RoomCreateRequestValidator : AbstractValidator<RoomCreateRequest>
{
    public RoomCreateRequestValidator()
    {
        RuleFor(x => x.Name).RequiredText(Resource.Name, FieldLengths.RoomName);

        RuleFor(x => x.Number)
            .GreaterThanOrEqualTo(0).WithMessage(string.Format(Resource.PropertyCannotBeNegative, Resource.Number));

        RuleFor(x => x.DepartmentId).RequiredReference(Resource.Department);
    }
}
