using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain;
using Microsoft.AspNetCore.Components.Authorization;

namespace BeHealthy.States
{
    public class PrivilegeStateService
    {
        private readonly IPrivilegeService _privilegeService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private Dictionary<string, bool> _privileges = new Dictionary<string, bool>();

        public PrivilegeStateService(IPrivilegeService privilegeService, AuthenticationStateProvider authenticationStateProvider)
        {
            _privilegeService = privilegeService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task LoadUserPrivileges()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userRole = Enum.Parse<UserRole>(authState.User.GetUserRole());

            //_privileges = await _privilegeService.GetPrivilegesForRoleAsync(userRole);
        }

        public async Task<bool> HasPrivilegeAsync(string privilegeName)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userRole = Enum.Parse<UserRole>(authState.User.GetUserRole());

            if (userRole == UserRole.Admin)
                return true;

            return _privileges.ContainsKey(privilegeName) && _privileges[privilegeName];
        }
    }
}
