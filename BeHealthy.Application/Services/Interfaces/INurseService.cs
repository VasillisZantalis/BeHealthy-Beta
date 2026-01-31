using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services.Interfaces;

public interface INurseService
{
    Task<PaginatedResult<NurseDto>> GetAllNursesAsync(QueryParameters? parameters = null);
    Task<NurseDto?> GetNurseByIdAsync(int id);
    Task<IEnumerable<NurseDto>> GetNursesOfPatientByUserId(string userId);
    Task<IEnumerable<NurseSimpleDto>> GetAllNursesSimpleAsync();
    Task<ProfileDto?> GetNurseProfileByUserIdAsync(string userId);
    Task<ServiceResponse> AddNurseAsync(NurseCreateDto nurse);
    Task<int> GetNurseCountAsync();
    Task<ServiceResponse> UpdateNurseAsync(NurseUpdateDto nurse);
    Task DeleteNurseAsync(int id);
}
