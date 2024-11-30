using BeHealthy.Shared.Locales;
using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Application.Dtos.Patient;

public class PatientForUpdateDto
{
    public int Id { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]

    public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]

    public string LastName { get; set; } = string.Empty;

    public string? Image { get; set; }

    public string UserId { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public int? DepartmentId { get; set; }
}
