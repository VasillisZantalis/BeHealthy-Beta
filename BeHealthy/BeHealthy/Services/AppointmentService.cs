using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Shared.Interfaces;
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

    public async Task AddAppointmentAsync(AppointmentForCreationDto appointmentDto)
    {
        var appointment = _mapper.Map<Appointment>(appointmentDto);
        await _unitOfWork.AppointmentRepository.AddAsync(appointment);
    }

    public async Task UpdateAppointmentAsync(int id, AppointmentForUpdateDto appointmentDto)
    {
        var appointment = _mapper.Map<Appointment>(appointmentDto);
        await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
    }

    public async Task DeleteAppointmentAsync(int id)
    {
        await _unitOfWork.AppointmentRepository.DeleteAsync(id);
    }
}

