using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Dtos.User;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Frontend.Services.Api;

public class NurseApiService : ApiClientBase, INurseService
{
    public NurseApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<PaginatedResult<NurseResponse>> GetAllNursesAsync(QueryParameters? parameters = null)
        => await GetAsync<PaginatedResult<NurseResponse>>($"nurses{ToQueryString(parameters)}") ?? new();

    public async Task<NurseResponse?> GetNurseByIdAsync(int id)
        => await GetAsync<NurseResponse>($"nurses/{id}");

    public async Task<IEnumerable<NurseResponse>> GetNursesOfPatientByUserId(string userId)
        => await GetListAsync<NurseResponse>($"nurses/patient/{userId}");

    public async Task<IEnumerable<NurseSimpleResponse>> GetAllNursesSimpleAsync()
        => await GetListAsync<NurseSimpleResponse>("nurses/simple");

    public async Task<ProfileResponse?> GetNurseProfileByUserIdAsync(string userId)
        => await GetAsync<ProfileResponse>($"nurses/{userId}/profile");

    public async Task<ServiceResponse> AddNurseAsync(NurseCreateRequest nurse)
        => await PostForResponseAsync("nurses", nurse);

    public async Task<int> GetNurseCountAsync()
        => await GetAsync<int>("nurses/count");

    public async Task<ServiceResponse> UpdateNurseAsync(NurseUpdateRequest nurse)
        => await PutForResponseAsync("nurses", nurse);

    public async Task DeleteNurseAsync(int id)
        => await DeleteAsync($"nurses/{id}");
}
