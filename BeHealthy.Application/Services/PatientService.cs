using AutoMapper;
using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Interfaces;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync(string? firstName = null, string? lastName = null)
    {
        var patients = await _unitOfWork.PatientRepository.GetAllPatientsAsync(firstName, lastName);
        return _mapper.Map<IEnumerable<PatientDto>>(patients);
    }

    public async Task<PatientDto> GetPatientByIdAsync(int id)
    {
        var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);
        return _mapper.Map<PatientDto>(patient);
    }

    public async Task AddPatientAsync(PatientForCreationDto patientDto)
    {
        var patient = _mapper.Map<Patient>(patientDto);
        await _unitOfWork.PatientRepository.AddAsync(patient);
    }

    public async Task UpdatePatientAsync(int id, PatientForUpdateDto patientDto)
    {
        var patient = _mapper.Map<Patient>(patientDto);
        await _unitOfWork.PatientRepository.UpdateAsync(patient);
    }

    public async Task DeletePatientAsync(int id)
    {
        await _unitOfWork.PatientRepository.DeletePatientAsync(id);
    }

    public async Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsByUserIdAsync(string userId)
    {
        var patientAppointments = await _unitOfWork.PatientRepository.GetPatientAppointmentsByUserIdAsync(userId);

        return _mapper.Map<IEnumerable<AppointmentDto>>(patientAppointments);
    }
}
