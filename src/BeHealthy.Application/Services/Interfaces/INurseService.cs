using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services.Interfaces;

public interface INurseService
{
    Task<PaginatedResult<NurseResponse>> GetAllNursesAsync(QueryParameters? parameters = null, CancellationToken cancellationToken = default);
    Task<NurseResponse?> GetNurseByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<NurseResponse>> GetNursesOfPatientByUserId(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<NurseSimpleResponse>> GetAllNursesSimpleAsync(CancellationToken cancellationToken = default);
    Task<ProfileResponse?> GetNurseProfileByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddNurseAsync(NurseCreateRequest nurse, CancellationToken cancellationToken = default);
    Task<int> GetNurseCountAsync(CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateNurseAsync(NurseUpdateRequest nurse, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteNurseAsync(int id, CancellationToken cancellationToken = default);
}
