using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Interfaces;
using BeHealthy.Application.Mappings;

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

    public async Task<DoctorDto> GetDoctorByIdAsync(int id)
    {
        var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id);
        return doctor.MapToDto();
    }

    public async Task AddDoctorAsync(DoctorForCreationDto doctorDto)
    {
        var doctor = doctorDto.MapToDomain();
        await _unitOfWork.DoctorRepository.AddAsync(doctor);
    }

    public async Task UpdateDoctorAsync(int id, DoctorForUpdateDto doctorDto)
    {
        var doctor = doctorDto.MapToDomain();
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
}

