using BeHealthy.Shared.Dtos.Room;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Rooms;

public class RoomUpdateRequestServerValidator : AbstractValidator<RoomUpdateRequest>
{
    public RoomUpdateRequestServerValidator(IDepartmentRepository departments)
    {
        RuleFor(x => x.DepartmentId).MustExist(departments, Resource.Department);
    }
}
