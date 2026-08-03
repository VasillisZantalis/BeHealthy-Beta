using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Department;

namespace BeHealthy.Frontend.Services.Api;

public class DepartmentApiService : ApiClientBase, IDepartmentService
{
    public DepartmentApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync()
        => await GetListAsync<DepartmentResponse>("departments");

    public async Task<DepartmentResponse> GetDepartmentByIdAsync(int id)
        => await GetAsync<DepartmentResponse>($"departments/{id}") ?? new();

    public async Task<ServiceResponse> AddDepartmentAsync(DepartmentCreateRequest departmentDto)
        => await PostForResponseAsync("departments", departmentDto);

    public async Task<ServiceResponse> UpdateDepartmentAsync(DepartmentUpdateRequest departmentDto)
        => await PutForResponseAsync("departments", departmentDto);

    public async Task<ServiceResponse> DeleteDepartmentAsync(int id)
        => await DeleteForResponseAsync($"departments/{id}");
}
