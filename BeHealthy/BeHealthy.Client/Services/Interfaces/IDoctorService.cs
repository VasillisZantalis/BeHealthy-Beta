using BeHealthy.Shared.Models.Dtos.Doctor;

namespace BeHealthy.Client.Services.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
    Task<DoctorDto>? GetDoctorByIdAsync(int id);
    Task AddDoctorAsync(DoctorForCreationDto doctorDto);
    Task UpdateDoctorAsync(int id, DoctorForUpdateDto doctorDto);
    Task DeleteDoctorAsync(int id);
    Task<List<DoctorDto>> GetDummyDoctorsAsync();
}
