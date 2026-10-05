using BeHealthy.Application.Services.Seeding;
using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Tests.UnitTests.Fakes;
using BeHealthy.Validation.Appointments;
using BeHealthy.Validation.Doctors;
using BeHealthy.Validation.Nurses;
using BeHealthy.Validation.Patients;
using FluentValidation.TestHelper;

namespace BeHealthy.Tests.UnitTests.Services;

public class SeedDataGeneratorTests
{
    private const int SampleSize = 50;

    private static readonly int[] DepartmentIds = [1, 2];
    private static readonly int[] SpecialtyIds = [3, 4];
    private static readonly int[] DoctorIds = [5, 6];
    private static readonly int[] PatientIds = [7, 8];
    private static readonly int[] NurseIds = [9];
    private static readonly int[] RoomIds = [10];

    private readonly SeedDataGenerator _sut = new(seed: 1234);

    [Fact]
    public async Task Doctor_PassesTheCreateValidator()
    {
        // Arrange
        var validator = new DoctorCreateRequestValidator(new FakeValidationSettingsProvider());

        for (int i = 0; i < SampleSize; i++)
        {
            // Act
            var doctor = _sut.Doctor(DepartmentIds, SpecialtyIds);
            var result = await validator.TestValidateAsync(doctor);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
            doctor.Email.ShouldEndWith($"@{SeedDataGenerator.EmailDomain}");
            doctor.SpecialtyId.ShouldNotBeNull();
            SpecialtyIds.ShouldContain(doctor.SpecialtyId!.Value);
        }
    }

    [Fact]
    public async Task Patient_PassesTheCreateValidator()
    {
        // Arrange
        var validator = new PatientCreateRequestValidator();

        for (int i = 0; i < SampleSize; i++)
        {
            // Act
            var result = await validator.TestValidateAsync(_sut.Patient(DepartmentIds));

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }

    [Fact]
    public async Task Nurse_PassesTheCreateValidator()
    {
        // Arrange
        var validator = new NurseCreateRequestValidator();

        for (int i = 0; i < SampleSize; i++)
        {
            // Act
            var result = await validator.TestValidateAsync(_sut.Nurse(DepartmentIds));

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }

    [Fact]
    public void Person_WithoutDepartments_HasNoDepartment()
    {
        // Act
        var patient = _sut.Patient([]);

        // Assert
        patient.DepartmentId.ShouldBeNull();
    }

    [Fact]
    public async Task Appointment_PassesTheCreateValidatorAndFallsOnAWeekdayDuringOfficeHours()
    {
        // Arrange
        var validator = new AppointmentCreateRequestValidator(new FakeValidationSettingsProvider());

        for (int i = 0; i < SampleSize; i++)
        {
            // Act
            var appointment = _sut.Appointment(DoctorIds, PatientIds, NurseIds, RoomIds);
            var result = await validator.TestValidateAsync(appointment);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
            appointment.AppointmentDate.DayOfWeek.ShouldNotBe(DayOfWeek.Saturday);
            appointment.AppointmentDate.DayOfWeek.ShouldNotBe(DayOfWeek.Sunday);
            appointment.AppointmentStartTime.ShouldBeGreaterThanOrEqualTo(new TimeOnly(8, 0));
            appointment.AppointmentEndTime.ShouldBeLessThanOrEqualTo(new TimeOnly(17, 30));
            appointment.Notes.ShouldNotBeNullOrWhiteSpace();
            DoctorIds.ShouldContain(appointment.DoctorId);
            PatientIds.ShouldContain(appointment.PatientId);
        }
    }

    [Fact]
    public void Appointment_InThePast_IsNeverScheduled()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        for (int i = 0; i < SampleSize; i++)
        {
            // Act
            var appointment = _sut.Appointment(DoctorIds, PatientIds, NurseIds, RoomIds);

            // Assert
            if (appointment.AppointmentDate < today)
            {
                appointment.Status.ShouldBeOneOf(AppointmentStatus.Completed, AppointmentStatus.Cancelled);
            }
        }
    }
}
