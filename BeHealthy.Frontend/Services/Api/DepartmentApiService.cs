using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Department;

namespace BeHealthy.Frontend.Services.Api;

public class DepartmentApiService : ApiClientBase, IDepartmentService
{
    public DepartmentApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        => await GetListAsync<DepartmentDto>("departments");

    public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
        => await GetAsync<DepartmentDto>($"departments/{id}") ?? new();

    public async Task<ServiceResponse> AddDepartmentAsync(DepartmentCreateDto departmentDto)
        => await PostForResponseAsync("departments", departmentDto);

    public async Task<ServiceResponse> UpdateDepartmentAsync(DepartmentUpdateDto departmentDto)
        => await PutForResponseAsync("departments", departmentDto);

    public async Task<ServiceResponse> DeleteDepartmentAsync(int id)
        => await DeleteForResponseAsync($"departments/{id}");
}
