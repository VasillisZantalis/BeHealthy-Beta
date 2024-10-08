using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;

    public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
    {
        //var appointment = await _appointmentRepository.GetAllAsync();
        var appointment = await _appointmentRepository.GetAllAppointmentsAsync();
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByDoctorIdAsync(int doctorId)
    {
        var appointment = await _appointmentRepository.GetAllAppointmentsByDoctorIdAsync(doctorId);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByPatientIdAsync(int patientId)
    {
        var appointment = await _appointmentRepository.GetAllAppointmentsByPatientIdAsync(patientId);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointment);
    }

    public async Task<AppointmentDto> GetAppointmentByIdAsync(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task AddAppointmentAsync(AppointmentForCreationDto appointmentDto)
    {
        var appointment = _mapper.Map<Appointment>(appointmentDto);
        await _appointmentRepository.AddAsync(appointment);
    }

    public async Task UpdateAppointmentAsync(AppointmentForUpdateDto appointmentDto)
    {
        var appointment = _mapper.Map<Appointment>(appointmentDto);
        await _appointmentRepository.UpdateAsync(appointment);
    }

    public async Task DeleteAppointmentAsync(int id)
    {
        await _appointmentRepository.DeleteAsync(id);
    }
}

