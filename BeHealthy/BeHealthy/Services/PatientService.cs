using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
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
        var patients = await _unitOfWork.PatientRepository.GetAllAsync();
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

    public async Task UpdatePatientAsync(PatientForUpdateDto patientDto)
    {
        var patient = _mapper.Map<Patient>(patientDto);
        await _unitOfWork.PatientRepository.UpdateAsync(patient);
    }

    public async Task DeletePatientAsync(int id)
    {
        await _unitOfWork.PatientRepository.DeleteAsync(id);
    }
}
