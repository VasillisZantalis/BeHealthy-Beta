using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain;
using BeHealthy.Domain.Interfaces;

namespace BeHealthy.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Dictionary<string, int>> GetUsersInRolesCount()
    {
        var result = new Dictionary<string, int>
        {
            { nameof(UserRole.Doctor), await _unitOfWork.DoctorRepository.GetCountAsync() },
            { nameof(UserRole.Patient), await _unitOfWork.PatientRepository.GetCountAsync() },
            { nameof(UserRole.Nurse), await _unitOfWork.NurseRepository.GetCountAsync() }
        };

        return result;
    }
}
