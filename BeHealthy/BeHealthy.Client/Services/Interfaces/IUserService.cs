using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Client.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<ApplicationUser>> GetAllDoctorsAsync();
    Task<IEnumerable<ApplicationUser>> GetAllPatientsAsync();
    Task<IEnumerable<ApplicationUser>> GetAllNursesAsync();
    Task<IEnumerable<ApplicationUser>> GetAllStaffAsync();
    Task DeleteUserAsync(string id);
}
