using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Patient;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    public PatientService(IPatientRepository patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
    {
        var patients = await _patientRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PatientDto>>(patients);
    }

    public async Task<PatientDto> GetPatientByIdAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);
        return _mapper.Map<PatientDto>(patient);
    }

    public async Task AddPatientAsync(PatientForCreationDto patientDto)
    {
        var patient = _mapper.Map<Patient>(patientDto);
        await _patientRepository.AddAsync(patient);
    }

    public async Task UpdatePatientAsync(PatientForUpdateDto patientDto)
    {
        var patient = _mapper.Map<Patient>(patientDto);
        await _patientRepository.UpdateAsync(patient);
    }

    public async Task DeletePatientAsync(int id)
    {
        await _patientRepository.DeleteAsync(id);
    }
}
