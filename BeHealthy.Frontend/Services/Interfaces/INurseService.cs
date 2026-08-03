using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Dtos.User;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface INurseService
{
    Task<PaginatedResult<NurseResponse>> GetAllNursesAsync(QueryParameters? parameters = null);
    Task<NurseResponse?> GetNurseByIdAsync(int id);
    Task<IEnumerable<NurseResponse>> GetNursesOfPatientByUserId(string userId);
    Task<IEnumerable<NurseSimpleResponse>> GetAllNursesSimpleAsync();
    Task<ProfileResponse?> GetNurseProfileByUserIdAsync(string userId);
    Task<ServiceResponse> AddNurseAsync(NurseCreateRequest nurse);
    Task<int> GetNurseCountAsync();
    Task<ServiceResponse> UpdateNurseAsync(NurseUpdateRequest nurse);
    Task DeleteNurseAsync(int id);
}
