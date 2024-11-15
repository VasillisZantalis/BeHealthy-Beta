using BeHealthy.Application.Dtos.Nurse;

namespace BeHealthy.Application.Services.Interfaces;

public interface INurseService
{
    Task<IEnumerable<NurseDto>> GetAllNursesAsync();
    Task<NurseDto> GetNurseByIdAsync(int id);
    Task AddNurseAsync(NurseForCreationDto nurse);
    Task UpdateNurseAsync(int id, NurseForUpdateDto nurse);
    Task DeleteNurseAsync(int id);
}
