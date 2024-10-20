using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Dtos.Patient;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
    {
        var patients = await _unitOfWork.PatientRepository.GetAllPatientsAsync();
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
