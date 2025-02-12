using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;
using BeHealthy.Shared.Parameters;
using System.Collections.Generic;

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
}
