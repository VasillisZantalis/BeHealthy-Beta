using BeHealthy.Front.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.MedicalRecord;

namespace BeHealthy.Front.Services.Api;

public class MedicalRecordApiService : ApiClientBase, IMedicalRecordService
{
    public MedicalRecordApiService(IHttpClientFactory httpClientFactory, ICurrentUserService currentUser) : base(httpClientFactory, currentUser) { }

    public async Task<IEnumerable<MedicalRecordResponse>> GetAllMedicalRecordsAsync()
        => await GetListAsync<MedicalRecordResponse>("medical-records");

    public async Task<MedicalRecordResponse?> GetMedicalRecordByIdAsync(int id)
        => await GetAsync<MedicalRecordResponse>($"medical-records/{id}");

    public async Task<IEnumerable<MedicalRecordResponse>> GetMedicalRecordsByPatientIdAsync(int patientId)
        => await GetListAsync<MedicalRecordResponse>($"medical-records/by-patient/{patientId}");

    public async Task<ServiceResponse> AddMedicalRecordAsync(MedicalRecordCreateRequest medicalRecordDto)
        => await PostForResponseAsync("medical-records", medicalRecordDto);

    public async Task<ServiceResponse> UpdateMedicalRecordAsync(MedicalRecordUpdateRequest medicalRecordDto)
        => await PutForResponseAsync($"medical-records/{medicalRecordDto.Id}", medicalRecordDto);

    public async Task DeleteMedicalRecordAsync(int id)
        => await DeleteAsync($"medical-records/{id}");

    public async Task<ServiceResponse> UpdateMedicalRecordNotesAsync(int id, MedicalRecordNotesUpdateRequest notesDto)
        => await PatchForResponseAsync($"medical-records/{id}/notes", notesDto);
}
