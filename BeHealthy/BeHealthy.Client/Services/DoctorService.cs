using BeHealthy.Client.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Doctor;
using System.Net.Http.Json;

namespace BeHealthy.Client.Services;

public class DoctorService : IDoctorService
{
    private readonly HttpClient _httpClient;

    public DoctorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
    {
        var doctors = await _httpClient.GetFromJsonAsync<IEnumerable<DoctorDto>>("api/doctors");
        return doctors ?? new List<DoctorDto>();
    }

    public async Task<DoctorDto>? GetDoctorByIdAsync(int id)
    {
        var doctors = await _httpClient.GetFromJsonAsync<DoctorDto>($"api/doctors/{id}");
        return doctors ?? null!;
    }

    public async Task AddDoctorAsync(DoctorForCreationDto doctorDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/doctors", doctorDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateDoctorAsync(int id, DoctorForUpdateDto doctorDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/doctors/{id}", doctorDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteDoctorAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/doctors/{id}");
        response.EnsureSuccessStatusCode();
    }


    public async Task<List<DoctorDto>> GetDummyDoctorsAsync()
    {
        await Task.Delay(1000);

        return new List<DoctorDto>
        {
            new DoctorDto
            {
                Id = 1,
                UserId = "user1-id",
                FirstName = "John",
                LastName = "Doe",
                Specialty = "Cardiology",
                PhoneNumber = "123-456-7890"
            },
            new DoctorDto
            {
                Id = 2,
                UserId = "user2-id",
                FirstName = "Jane",
                LastName = "Smith",
                Specialty = "Neurology",
                PhoneNumber = "123-456-7891"
            },
            new DoctorDto
            {
                Id = 3,
                UserId = "user3-id",
                FirstName = "Alice",
                LastName = "Brown",
                Specialty = "Pediatrics",
                PhoneNumber = "123-456-7892"
            },
            new DoctorDto
            {
                Id = 4,
                UserId = "user4-id",
                FirstName = "Michael",
                LastName = "Johnson",
                Specialty = "Orthopedics",
                PhoneNumber = "123-456-7893"
            },
            new DoctorDto
            {
                Id = 5,
                UserId = "user5-id",
                FirstName = "Emily",
                LastName = "Davis",
                Specialty = "Dermatology",
                PhoneNumber = "123-456-7894"
            }
        };
    }
}
