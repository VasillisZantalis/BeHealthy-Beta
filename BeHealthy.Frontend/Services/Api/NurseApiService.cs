using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Dtos.User;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Frontend.Services.Api;

public class NurseApiService : ApiClientBase, INurseService
{
    public NurseApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<PaginatedResult<NurseDto>> GetAllNursesAsync(QueryParameters? parameters = null)
        => await GetAsync<PaginatedResult<NurseDto>>($"nurses{ToQueryString(parameters)}") ?? new();

    public async Task<NurseDto?> GetNurseByIdAsync(int id)
        => await GetAsync<NurseDto>($"nurses/{id}");

    public async Task<IEnumerable<NurseDto>> GetNursesOfPatientByUserId(string userId)
        => await GetListAsync<NurseDto>($"nurses/patient/{userId}");

    public async Task<IEnumerable<NurseSimpleDto>> GetAllNursesSimpleAsync()
        => await GetListAsync<NurseSimpleDto>("nurses/simple");

    public async Task<ProfileDto?> GetNurseProfileByUserIdAsync(string userId)
        => await GetAsync<ProfileDto>($"nurses/{userId}/profile");

    public async Task<ServiceResponse> AddNurseAsync(NurseCreateDto nurse)
        => await PostForResponseAsync("nurses", nurse);

    public async Task<int> GetNurseCountAsync()
        => await GetAsync<int>("nurses/count");

    public async Task<ServiceResponse> UpdateNurseAsync(NurseUpdateDto nurse)
        => await PutForResponseAsync("nurses", nurse);

    public async Task DeleteNurseAsync(int id)
        => await DeleteAsync($"nurses/{id}");
}
