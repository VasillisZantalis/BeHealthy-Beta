using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;

    public AppointmentService(IUnitOfWork unitOfWork, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
    {
        var appointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsAsync();
        return appointments.MapToDto();
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByDoctorIdAsync(int doctorId)
    {
        var appointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByDoctorIdAsync(doctorId);
        return appointments.MapToDto();
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByPatientIdAsync(int patientId)
    {
        var appointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByPatientIdAsync(patientId);
        return appointments.MapToDto();
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByUserIdAsync(string userId)
    {
        var appointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByUserIdAsync(userId);
        return appointments.MapToDto();
    }

    public async Task<AppointmentDto> GetAppointmentByIdAsync(int id)
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
        return appointment.MapToDto();
    }

    public async Task<ServiceResponse> AddAppointmentAsync(AppointmentForCreationDto appointmentDto)
    {
        var appointment = appointmentDto.MapToDomain();

        var conflictCheck = await CheckForConflictingAppointmentsAsync(
            appointmentDto.DoctorId,
            appointmentDto.PatientId,
            appointmentDto.NurseId,
            appointmentDto.RoomId,
            appointmentDto.AppointmentDate,
            appointmentDto.Duration);

        if (!conflictCheck.Success)
        {
            return conflictCheck;
        }

        await _unitOfWork.AppointmentRepository.AddAsync(appointment);

        return appointment.Id > 0
            ? ServiceResponse.Successful()
            : ServiceResponse.Failed(Resource.SomethingWentWrong);
    }

    public async Task<ServiceResponse> UpdateAppointmentAsync(int id, AppointmentForUpdateDto appointmentDto)
    {
        var appointment = appointmentDto.MapToDomain();

        var conflictCheck = await CheckForConflictingAppointmentsAsync(
            appointmentDto.DoctorId,
            appointmentDto.PatientId,
            appointmentDto.NurseId,
            appointmentDto.RoomId,
            appointmentDto.AppointmentDate,
            appointmentDto.Duration,
            appointmentDto.Id);

        if (!conflictCheck.Success)
        {
            return conflictCheck;
        }

        await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);

        return ServiceResponse.Successful();
    }

    public async Task DeleteAppointmentAsync(int id)
    {
        await _unitOfWork.AppointmentRepository.DeleteAsync(id);
    }

    private async Task<ServiceResponse> CheckForConflictingAppointmentsAsync(
        int doctorId,
        int patientId, 
        int? nurseId,
        int? roomId,
        DateTime appointmentDate, 
        int duration, 
        int? appointmentId = null)
    {
        var doctorsAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByDoctorIdAsync(doctorId);

        var newAppointmentStart = appointmentDate;
        var newAppointmentEnd = newAppointmentStart.AddMinutes(duration);

        var doctorConflict = doctorsAppointments.FirstOrDefault(existingAppointment =>
        {
            if (appointmentId.HasValue && existingAppointment.Id == appointmentId.Value)
                return false;

            var existingStart = existingAppointment.AppointmentDate;
            var existingEnd = existingStart.AddMinutes(existingAppointment.Duration);

            return newAppointmentStart < existingEnd && newAppointmentEnd > existingStart;
        });

        if (doctorConflict != null)
        {
            var errorMessage = string.Format(
                Resource.AppointmentExistsForDoctor,
                doctorConflict?.Doctor?.FullName,
                doctorConflict?.AppointmentDate.ToString("HH:mm"),
                doctorConflict?.AppointmentDate.AddMinutes(doctorConflict.Duration).ToString("HH:mm")
            );
            return ServiceResponse.Failed(errorMessage);
        }

        var patientAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByPatientIdAsync(patientId);

        var patientConflict = patientAppointments.FirstOrDefault(existingAppointment =>
        {
            if (appointmentId.HasValue && existingAppointment.Id == appointmentId.Value)
                return false;

            var existingStart = existingAppointment.AppointmentDate;
            var existingEnd = existingStart.AddMinutes(existingAppointment.Duration);

            return newAppointmentStart < existingEnd && newAppointmentEnd > existingStart;
        });

        if (patientConflict != null)
        {
            var errorMessage = string.Format(
                Resource.AppointmentExistsForPatient,
                patientConflict?.Patient?.FullName,
                patientConflict?.AppointmentDate.ToString("HH:mm"),
                patientConflict?.AppointmentDate.AddMinutes(patientConflict.Duration).ToString("HH:mm")
            );
            return ServiceResponse.Failed(errorMessage);
        }

        var nurseAppointments = nurseId.HasValue
            ? await _unitOfWork.AppointmentRepository.GetAllAppointmentsByNurseIdAsync(nurseId.Value)
            : Enumerable.Empty<Appointment>();

        var nurseConflict = nurseAppointments.FirstOrDefault(existingAppointment =>
        {
            if (appointmentId.HasValue && existingAppointment.Id == appointmentId.Value)
                return false;

            var existingStart = existingAppointment.AppointmentDate;
            var existingEnd = existingStart.AddMinutes(existingAppointment.Duration);

            return newAppointmentStart < existingEnd && newAppointmentEnd > existingStart;
        });

        if (nurseConflict != null)
        {
            var errorMessage = string.Format(
                Resource.AppointmentExistsForPatient,
                nurseConflict?.Nurse?.FullName,
                nurseConflict?.AppointmentDate.ToString("HH:mm"),
                nurseConflict?.AppointmentDate.AddMinutes(nurseConflict.Duration).ToString("HH:mm")
            );
            return ServiceResponse.Failed(errorMessage);
        }

        var roomsAppointments = roomId.HasValue
            ? await _unitOfWork.RoomRepository.GetRoomAppointmentsAsync(roomId.Value)
            : Enumerable.Empty<Appointment>();

        var roomsConflict = roomsAppointments.FirstOrDefault(existingAppointment =>
        {
            if (appointmentId.HasValue && existingAppointment.Id == appointmentId.Value)
                return false;

            var existingStart = existingAppointment.AppointmentDate;
            var existingEnd = existingStart.AddMinutes(existingAppointment.Duration);

            return newAppointmentStart < existingEnd && newAppointmentEnd > existingStart;
        });

        if (roomsConflict != null)
        {
            return ServiceResponse.Failed(Resource.RoomIsBookedAtThatTime);
        }

        return ServiceResponse.Successful();
    }
}

