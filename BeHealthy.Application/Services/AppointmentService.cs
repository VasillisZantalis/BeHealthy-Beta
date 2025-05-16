using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

    public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
        return appointment?.MapToDto();
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
            appointmentDto.AppointmentStartTime,
            appointmentDto.AppointmentEndTime);

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
            appointmentDto.AppointmentStartTime,
            appointmentDto.AppointmentEndTime,
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

    public async Task<Dictionary<AppointmentReason, int>> GetAppointmentReasonCounts()
    {
        var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();

        var groupedByReason = appointments
            .GroupBy(x => x.Reason)
            .Select(x => new
            {
                x.Key,
                Count = x.Count()
            })
            .ToDictionary(k => k.Key, v => v.Count);

        return groupedByReason;
    }

    private async Task<ServiceResponse> CheckForConflictingAppointmentsAsync(
        int doctorId,
        int patientId, 
        int? nurseId,
        int? roomId,
        DateOnly appointmentDate,
        TimeOnly appointmentStartTime,
        TimeOnly appointmentEndTime,
        int? appointmentId = null)
    {
        DateTime newStart = appointmentDate.ToDateTime(appointmentStartTime);
        DateTime newEnd = appointmentDate.ToDateTime(appointmentEndTime);

        var doctorAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByDoctorIdAsync(doctorId);
        var doctorConflict =  FindConflict(doctorAppointments, newStart, newEnd, appointmentId);

        if (doctorConflict != null)
        {
            var errorMessage = string.Format(
                Resource.AppointmentExistsForDoctor,
                doctorConflict.Doctor?.FullName,
                doctorConflict.AppointmentStartTime.ToShortTimeString(),
                doctorConflict.AppointmentEndTime.ToShortTimeString()
            );
            return ServiceResponse.Failed(errorMessage);
        }

        var patientAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByPatientIdAsync(patientId);
        var patientConflict = FindConflict(patientAppointments, newStart, newEnd, appointmentId);

        if (patientConflict != null)
        {
            var errorMessage = string.Format(
                Resource.AppointmentExistsForPatient,
                patientConflict.Patient?.FullName,
                patientConflict.AppointmentStartTime.ToShortTimeString(),
                patientConflict.AppointmentEndTime.ToShortTimeString()
            );
            return ServiceResponse.Failed(errorMessage);
        }

        if (nurseId.HasValue)
        {
            var nurseAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByNurseIdAsync(nurseId.Value);
            var nurseConflict = FindConflict(nurseAppointments, newStart, newEnd, appointmentId);

            if (nurseConflict != null)
            {
                var errorMessage = string.Format(
                    Resource.AppointmentExistsForNurse,
                    nurseConflict.Nurse?.FullName,
                    nurseConflict.AppointmentStartTime.ToShortTimeString(),
                    nurseConflict.AppointmentEndTime.ToShortTimeString()
                );
                return ServiceResponse.Failed(errorMessage);
            }
        }

        if (roomId.HasValue)
        {
            var roomAppointments = await _unitOfWork.RoomRepository.GetRoomAppointmentsAsync(roomId.Value);
            var roomConflict = FindConflict(roomAppointments, newStart, newEnd, appointmentId);

            if (roomConflict != null)
            {
                return ServiceResponse.Failed(Resource.RoomIsBookedAtThatTime);
            }
        }

        return ServiceResponse.Successful();
    }

    private Appointment? FindConflict(IEnumerable<Appointment> appointments, DateTime newStart, DateTime newEnd, int? excludeId)
    {
        return appointments.FirstOrDefault(existing =>
        {
            if (excludeId.HasValue && existing.Id == excludeId.Value)
                return false;

            DateTime existingStart = existing.AppointmentDate.ToDateTime(existing.AppointmentStartTime);
            DateTime existingEnd = existing.AppointmentDate.ToDateTime(existing.AppointmentEndTime);

            return newStart <= existingEnd && newEnd >= existingStart;
        });
    }
}

