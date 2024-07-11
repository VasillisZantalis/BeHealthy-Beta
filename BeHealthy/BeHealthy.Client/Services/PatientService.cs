using BeHealthy.Client.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Patient;
using System.Net.Http.Json;

namespace BeHealthy.Client.Services;

public class PatientService : IPatientService
{
    private readonly HttpClient _httpClient;

    public PatientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
    {
        var patients = await _httpClient.GetFromJsonAsync<IEnumerable<PatientDto>>("api/patients");
        return patients ?? new List<PatientDto>();
    }

    public async Task<PatientDto>? GetPatientByIdAsync(int id)
    {
        var patients = await _httpClient.GetFromJsonAsync<PatientDto>($"api/patients/{id}");
        return patients ?? null!;
    }

    public async Task AddPatientAsync(PatientForCreationDto patientDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/patients", patientDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePatientAsync(int id, PatientForUpdateDto patientDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/patients/{id}", patientDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePatientAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/patients/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<PatientDto>> GetDummyPatientsAsync()
    {
        await Task.Delay(1000);

        return new List<PatientDto>
        {
            new PatientDto
            {
                Id = 1,
                UserId = "user1-id",
                FirstName = "Alice",
                LastName = "Johnson",
                DateOfBirth = new DateTime(1990, 5, 15),
                Gender = "Female",
                Address = "123 Main St, Springfield",
                PhoneNumber = "123-456-7890"
            },
            new PatientDto
            {
                Id = 2,
                UserId = "user2-id",
                FirstName = "Bob",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 3, 22),
                Gender = "Male",
                Address = "456 Elm St, Springfield",
                PhoneNumber = "123-456-7891"
            },
            new PatientDto
            {
                Id = 3,
                UserId = "user3-id",
                FirstName = "Charlie",
                LastName = "Brown",
                DateOfBirth = new DateTime(2000, 7, 10),
                Gender = "Non-binary",
                Address = "789 Oak St, Springfield",
                PhoneNumber = "123-456-7892"
            },
            new PatientDto
            {
                Id = 4,
                UserId = "user4-id",
                FirstName = "David",
                LastName = "Davis",
                DateOfBirth = new DateTime(1978, 12, 30),
                Gender = "Male",
                Address = "101 Pine St, Springfield",
                PhoneNumber = "123-456-7893"
            },
            new PatientDto
            {
                Id = 5,
                UserId = "user5-id",
                FirstName = "Eve",
                LastName = "Miller",
                DateOfBirth = new DateTime(1995, 8, 25),
                Gender = "Female",
                Address = "202 Maple St, Springfield",
                PhoneNumber = "123-456-7894"
            }
        };
    }
}
