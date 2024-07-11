using BeHealthy.Shared.Models.Dtos.Nurse;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services.Interfaces;

public interface INurseService
{
    Task<IEnumerable<Nurse>> GetAllNursesAsync();
    Task<Nurse> GetNurseByIdAsync(int id);
    Task AddNurseAsync(NurseForCreationDto nurse);
    Task UpdateNurseAsync(NurseForUpdateDto nurse);
    Task DeleteNurseAsync(int id);
}
