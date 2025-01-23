using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Dtos.User;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;

namespace BeHealthy.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
    {
        var doctors = await _unitOfWork.DoctorRepository.GetAllDoctorsAsync();
        return doctors.MapToDto();
    }

    public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
    {
        var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id);
        return doctor?.MapToDto();
    }

    public async Task<ServiceResponse> AddDoctorAsync(DoctorForCreationDto doctorDto)
    {
        try
        {
            var doctor = doctorDto.MapToDomain();
            await _unitOfWork.DoctorRepository.AddAsync(doctor);

            return ServiceResponse.Successful();
        }
        catch (Exception ex)
        {
            return ServiceResponse.Failed();
        }
    }

    public async Task UpdateDoctorAsync(int id, DoctorForUpdateDto doctorDto)
    {
        var doctor = doctorDto.MapToDomain();

        if (await _unitOfWork.SpecialtyRepository.ExistsAsync(id))
            return;

        await _unitOfWork.DoctorRepository.UpdateAsync(doctor);
    }

    public async Task DeleteDoctorAsync(int id)
    {
        await _unitOfWork.DoctorRepository.DeleteDoctorAsync(id);
    }

    public async Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsByUserIdAsync(string userId)
    {
        var doctorAppointments = await _unitOfWork.DoctorRepository.GetDoctorAppointmentsByUserIdAsync(userId);
        return doctorAppointments.MapToDto();
    }

    public async Task<ProfileDto?> GetDoctorProfileByUserIdAsync(string userId)
    {
        var doctor = await _unitOfWork.DoctorRepository.GetDoctorByUserIdAsync(userId);

        if (doctor is null) return null;

        var profile = new ProfileDto
        {
            Id = doctor.Id,
            UserId = doctor.UserId,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Specialty = doctor.Specialty?.Name,
            Image = doctor.Image,
            Email = doctor.User?.Email,
            PhoneNumber = doctor.User?.PhoneNumber,
        };

        return profile;
    }

    public async Task<IEnumerable<PatientDto>> GetMyPatientsAsync(string userId)
    {
        var patients = new List<Patient>();

        var doctor = await _unitOfWork.DoctorRepository.GetDoctorByUserIdAsync(userId);

        if (doctor is null)
            return Enumerable.Empty<PatientDto>();

        var doctorAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByDoctorIdAsync(doctor.Id);

        List<int> patientIds = doctorAppointments
            .Select(x => x.PatientId)
            .Distinct()
            .ToList();

        if (patientIds.Any())
        {
            var treatedPatients = await _unitOfWork.PatientRepository.FindAsync(w => patientIds.Contains(w.Id));
            patients.AddRange(treatedPatients);
        }

        var isSupervisorDoctor = await _unitOfWork.DoctorRepository.IsDoctorHeadOfDepartmentAsync(doctor.Id);

        if (isSupervisorDoctor)
        {
            var departmentId = doctor.DepartmentId ?? 0;
            var departmentPatients = await _unitOfWork.PatientRepository.GetPatientsByDepartmentIdAsync(departmentId);

            patients.AddRange(departmentPatients);
        }

        var distinctPatients = patients
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .ToList();

        return distinctPatients.MapToDto();
    }
}

