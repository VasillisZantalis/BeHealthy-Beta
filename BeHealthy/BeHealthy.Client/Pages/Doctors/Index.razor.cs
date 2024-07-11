using BeHealthy.Client.Services;
using BeHealthy.Client.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Client.Pages.Doctors;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;

    private List<ApplicationUser> _doctors { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _createUserHref = "Account/Register?role=Doctor";
        _doctors = (await _userService.GetAllDoctorsAsync()).ToList();
    }

}
