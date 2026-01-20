using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Dtos.Nurse;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services.Interfaces;

public interface INurseService
{
    Task<IEnumerable<NurseDto>> GetAllNursesAsync(QueryParameters? parameters = null);
    Task<NurseDto?> GetNurseByIdAsync(int id);
    Task<IEnumerable<NurseDto>> GetNursesOfPatientByUserId(string userId, QueryParameters? parameters = null);
    Task<ServiceResponse> AddNurseAsync(NurseCreateDto nurse);
    Task<int> GetNurseCountAsync();
    Task<ServiceResponse> UpdateNurseAsync(NurseUpdateDto nurse);
    Task DeleteNurseAsync(int id);
}
