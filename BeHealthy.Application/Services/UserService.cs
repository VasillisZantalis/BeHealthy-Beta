using BeHealthy.Application.Interfaces;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Identity;

namespace BeHealthy.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;

    public UserService(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _userStore = userStore;
    }

    public async Task<ServiceResponse> CreateApplicationUser(ApplicationUser applicationUser, string password, CancellationToken cancellationToken = default)
    {
        await _userManager.SetEmailAsync(applicationUser, applicationUser.Email);
        await _userStore.SetUserNameAsync(applicationUser, applicationUser.Email, cancellationToken);

        var result = await _userManager.CreateAsync(applicationUser, password);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return ServiceResponse.Failed(errorMessage);
        }

        return ServiceResponse.Successful();
    }


    public async Task<ServiceResponse> AddUserToRoleAsync(ApplicationUser user, UserRole role)
    {
        if (user == null)
        {
            return ServiceResponse.Failed("User cannot be null");
        }

        if (!Enum.IsDefined(typeof(UserRole), role))
        {
            return ServiceResponse.Failed("Invalid user role");
        }

        var result = await _userManager.AddToRoleAsync(user, role.ToString());

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return ServiceResponse.Failed(errorMessage);
        }

        return ServiceResponse.Successful();
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

    public async Task<ServiceResponse> RemoveUserFromRoleAsync(ApplicationUser user, UserRole role)
    {
        if (user == null)
        {
            return ServiceResponse.Failed("User cannot be null");
        }
        if (!Enum.IsDefined(typeof(UserRole), role))
        {
            return ServiceResponse.Failed("Invalid user role");
        }
        var result = await _userManager.RemoveFromRoleAsync(user, role.ToString());
        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return ServiceResponse.Failed(errorMessage);
        }
        return ServiceResponse.Successful();
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return null;
        }
        var user = await _userManager.FindByIdAsync(userId);
        return user;
    }

    public async Task<ServiceResponse> DeleteUserAsync(ApplicationUser applicationUser)
    {
        if (applicationUser == null)
        {
            return ServiceResponse.Failed("User cannot be null");
        }

        // Ensure the user exists
        var existingUser = await _userManager.FindByIdAsync(applicationUser.Id);
        if (existingUser == null)
        {
            return ServiceResponse.Failed("User not found");
        }

        var result = await _userManager.DeleteAsync(applicationUser);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return ServiceResponse.Failed(errorMessage);
        }

        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> UpdateUserAsync(ApplicationUser applicationUser, CancellationToken cancellationToken = default)
    {
        var result = await _userManager.UpdateAsync(applicationUser);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return ServiceResponse.Failed(errorMessage);
        }

        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> CreateAdminAsync(ApplicationUser applicationUser, string password, CancellationToken cancellationToken = default)
    {
        await _userManager.SetEmailAsync(applicationUser, applicationUser.Email);
        await _userStore.SetUserNameAsync(applicationUser, applicationUser.Email, cancellationToken);

        var result = await _userManager.CreateAsync(applicationUser, password);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return ServiceResponse.Failed(errorMessage);
        }

        var assignToAdminResult = await AddUserToRoleAsync(applicationUser, UserRole.Admin);
        if (!assignToAdminResult.Success)
            return ServiceResponse.Failed(assignToAdminResult.ErrorMessage!);

        return ServiceResponse.Successful();
    }
}
