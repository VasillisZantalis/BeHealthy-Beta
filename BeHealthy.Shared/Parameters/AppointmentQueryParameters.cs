namespace BeHealthy.Shared.Parameters;

public class AppointmentQueryParameters : QueryParameters
{
    public int? DoctorId { get;set; }
    public int? PatientId { get; set; }
}
