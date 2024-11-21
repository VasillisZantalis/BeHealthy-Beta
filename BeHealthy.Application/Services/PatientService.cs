using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Interfaces;

namespace BeHealthy.Application.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync(string? firstName = null, string? lastName = null)
    {
        var patients = await _unitOfWork.PatientRepository.GetAllPatientsAsync(firstName, lastName);
        return patients.MapToDto();
    }

    public async Task<PatientDto> GetPatientByIdAsync(int id)
    {
        var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);
        return patient.MapToDto();
    }

    public async Task AddPatientAsync(PatientForCreationDto patientDto)
    {
        var patient = patientDto.MapToDomain();
        await _unitOfWork.PatientRepository.AddAsync(patient);
    }

    public async Task UpdatePatientAsync(int id, PatientForUpdateDto patientDto)
    {
        var patient = patientDto.MapToDomain();
        await _unitOfWork.PatientRepository.UpdateAsync(patient);
    }

    public async Task DeletePatientAsync(int id)
    {
        await _unitOfWork.PatientRepository.DeletePatientAsync(id);
    }

    public async Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsByUserIdAsync(string userId)
    {
        var patientAppointments = await _unitOfWork.PatientRepository.GetPatientAppointmentsByUserIdAsync(userId);
        return patientAppointments.MapToDto();
    }
}
