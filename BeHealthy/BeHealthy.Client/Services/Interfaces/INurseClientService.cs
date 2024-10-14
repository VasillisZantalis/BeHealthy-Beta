using BeHealthy.Shared.Models.Dtos.Nurse;

namespace BeHealthy.Client.Services.Interfaces;

public interface INurseClientService
{
    Task AddNurseAsync(NurseForCreationDto nurseDto);
    Task UpdateNurseAsync(int id, NurseForUpdateDto nurseDto);
    Task DeleteNurseAsync(int id);
}
