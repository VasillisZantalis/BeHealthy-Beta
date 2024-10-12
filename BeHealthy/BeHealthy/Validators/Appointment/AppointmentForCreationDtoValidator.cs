using BeHealthy.Shared.Models.Dtos.Appointment;
using FluentValidation;

namespace BeHealthy.Validators.Appointment;

public class AppointmentForCreationDtoValidator : AbstractValidator<AppointmentForCreationDto>
{
    public AppointmentForCreationDtoValidator()
    {
        RuleFor(p => p.PatientId)
            .NotEmpty()
            .WithMessage("Patient is Required");

        RuleFor(p => p.DoctorId)
            .NotEmpty()
            .WithMessage("Doctor is Required");

        RuleFor(p => p.AppointmentDate)
            .NotEmpty()
            .WithMessage("Date is Required");
    }
}
