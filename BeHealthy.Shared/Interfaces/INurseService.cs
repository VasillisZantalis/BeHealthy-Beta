using BeHealthy.Shared.Models.Dtos.Nurse;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Shared.Interfaces;

public interface INurseService
{
    Task<IEnumerable<NurseDto>> GetAllNursesAsync();
    Task<NurseDto> GetNurseByIdAsync(int id);
    Task AddNurseAsync(NurseForCreationDto nurse);
    Task UpdateNurseAsync(int id, NurseForUpdateDto nurse);
    Task DeleteNurseAsync(int id);
}
