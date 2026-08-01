using BeHealthy.Shared.Dtos.Room;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validations.Rooms;

public class RoomDtoValidator : AbstractValidator<RoomDto>
{
    public RoomDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage(string.Format(Resource.PropertyRequired, Resource.Name));

        RuleFor(r => r.DepartmentId)
            .NotEmpty()
            .WithMessage(string.Format(Resource.PropertyRequired, Resource.Department));
    }
}
