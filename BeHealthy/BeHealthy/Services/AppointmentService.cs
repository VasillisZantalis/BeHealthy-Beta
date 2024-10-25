using AutoMapper;
using BeHealthy.Persistance;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetAllAppointmentsAsync();
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByDoctorIdAsync(int doctorId)
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByDoctorIdAsync(doctorId);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByPatientIdAsync(int patientId)
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByPatientIdAsync(patientId);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByUserIdAsync(string userId)
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointment);
    }

    public async Task<AppointmentDto> GetAppointmentByIdAsync(int id)
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<ServiceResponse> AddAppointmentAsync(AppointmentForCreationDto appointmentDto)
    {
        var appointment = _mapper.Map<Appointment>(appointmentDto);

        var conflictCheck = await CheckForConflictingAppointmentsAsync(appointmentDto.DoctorId, appointmentDto.AppointmentDate, appointmentDto.Duration);
        if (!conflictCheck.Success)
        {
            return conflictCheck;
        }

        await _unitOfWork.AppointmentRepository.AddAsync(appointment);
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> UpdateAppointmentAsync(int id, AppointmentForUpdateDto appointmentDto)
    {
        var appointment = _mapper.Map<Appointment>(appointmentDto);

        var conflictCheck = await CheckForConflictingAppointmentsAsync(appointmentDto.DoctorId, appointmentDto.AppointmentDate, appointmentDto.Duration);
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

    private async Task<ServiceResponse> CheckForConflictingAppointmentsAsync(int doctorId, DateTime appointmentDate, int duration, int? appointmentId = null)
    {
        var doctorsAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByDoctorIdAsync(doctorId);

        var newAppointmentStart = appointmentDate;
        var newAppointmentEnd = newAppointmentStart.AddMinutes(duration);

        var conflictingAppointment = doctorsAppointments.FirstOrDefault(existingAppointment =>
        {
            if (appointmentId.HasValue && existingAppointment.Id == appointmentId.Value)
            {
                return false;
            }

            var existingStart = existingAppointment.AppointmentDate;
            var existingEnd = existingStart.AddMinutes(existingAppointment.Duration);

            return newAppointmentStart < existingEnd && newAppointmentEnd > existingStart;
        });

        if (conflictingAppointment != null)
        {
            var errorMessage = $"An appointment already exists for this doctor from {conflictingAppointment.AppointmentDate:HH:mm} to {conflictingAppointment.AppointmentDate.AddMinutes(conflictingAppointment.Duration):HH:mm}. Please choose a different time.";
            return ServiceResponse.Failed(errorMessage);
        }

        return ServiceResponse.Successful();
    }
}

