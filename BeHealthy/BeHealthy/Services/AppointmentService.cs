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
    private readonly IValidationService _validationService;

    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationService = validationService;
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
        //var validationResponse = await _validationService.ValidateAsync(appointmentDto);

        //if (!validationResponse.Success)
        //{
        //    return validationResponse;
        //}
        var appointment = _mapper.Map<Appointment>(appointmentDto);

        var conflictCheck = await CheckForConflictingAppointmentsAsync(
            appointmentDto.DoctorId, 
            appointmentDto.PatientId, 
            appointmentDto.AppointmentDate, 
            appointmentDto.Duration);

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

        var conflictCheck = await CheckForConflictingAppointmentsAsync(
            appointmentDto.DoctorId, 
            appointmentDto.PatientId, 
            appointmentDto.AppointmentDate, 
            appointmentDto.Duration);

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

    private async Task<ServiceResponse> CheckForConflictingAppointmentsAsync(int doctorId, int patientId, DateTime appointmentDate, int duration, int? appointmentId = null)
    {
        var doctorsAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByDoctorIdAsync(doctorId);
        var patientAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByPatientIdAsync(patientId);

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

        var patientConflict = patientAppointments.FirstOrDefault(existingAppointment =>
        {
            if (appointmentId.HasValue && existingAppointment.Id == appointmentId.Value)
                return false;

            var existingStart = existingAppointment.AppointmentDate;
            var existingEnd = existingStart.AddMinutes(existingAppointment.Duration);

            return newAppointmentStart < existingEnd && newAppointmentEnd > existingStart;
        });

        if (doctorConflict != null)
        {
            var errorMessage = $"An appointment already exists for doctor {doctorConflict?.Doctor?.FullName} from {doctorConflict?.AppointmentDate:HH:mm} to {doctorConflict?.AppointmentDate.AddMinutes(doctorConflict.Duration):HH:mm}. Please choose a different time.";
            return ServiceResponse.Failed(errorMessage);
        }

        if (patientConflict != null)
        {
            var errorMessage = $"An appointment already exists for patient {patientConflict?.Patient?.FullName} from {patientConflict?.AppointmentDate:HH:mm} to {patientConflict?.AppointmentDate.AddMinutes(patientConflict.Duration):HH:mm}. Please choose a different time.";
            return ServiceResponse.Failed(errorMessage);
        }

        return ServiceResponse.Successful();
    }
}

