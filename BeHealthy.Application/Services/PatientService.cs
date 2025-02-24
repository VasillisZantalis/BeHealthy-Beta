using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync(PatientSearchingParameters patientSearchingParameters)
    {
        var patients = await _unitOfWork.PatientRepository.GetAllPatientsAsync(patientSearchingParameters);
        return patients.MapToDto();
    }

    public async Task<PatientDto?> GetPatientByIdAsync(int id)
    {
        var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);
        return patient?.MapToDto();
    }

    public async Task<ServiceResponse> AddPatientAsync(PatientForCreationDto patientDto)
    {
        try
        {
            var patient = patientDto.MapToDomain();
            await _unitOfWork.PatientRepository.AddAsync(patient);
            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed();
        }
    }

    public async Task UpdatePatientAsync(int id, PatientForUpdateDto patientDto)
    {
        var patient = patientDto.MapToDomain();

        if (!await _unitOfWork.PatientRepository.ExistsAsync(id)
            || patient is null)
            return;

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

    public async Task<IEnumerable<DoctorDto>> GetMyDoctorsAsync(string userId)
    {
        var doctors = new List<Doctor>();

        var patient = await _unitOfWork.PatientRepository.GetByUserIdAsync(userId);

        if (patient is null)
            return Enumerable.Empty<DoctorDto>();

        var patientAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByPatientIdAsync(patient.Id);

        var doctorIds = patientAppointments
            .Select(s => s.DoctorId)
            .Distinct()
            .ToList();

        if (doctorIds.Any())
        {
            var treatingDoctors = await _unitOfWork.DoctorRepository.FindWithIncludesAsync(
                w => doctorIds.Contains(w.Id),
                false,
                w => w.User!);

            doctors.AddRange(treatingDoctors);
        }

        return doctors.MapToDto();
    }

    public Task<int> GetPatientCountAsync()
    {
        return _unitOfWork.PatientRepository.GetCountAsync();
    }
}
