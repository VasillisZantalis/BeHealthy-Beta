using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Dtos.Nurse;

namespace BeHealthy.Application.Services.Interfaces;

public interface INurseService
{
    Task<IEnumerable<NurseDto>> GetAllNursesAsync();
    Task<NurseDto?> GetNurseByIdAsync(int id);
    Task<IEnumerable<NurseDto>> GetNursesOfPatientByUserId(string userId);
    Task<ServiceResponse> AddNurseAsync(NurseForCreationDto nurse);
    Task<int> GetNurseCountAsync();
    Task UpdateNurseAsync(int id, NurseForUpdateDto nurse);
    Task DeleteNurseAsync(int id);
}
