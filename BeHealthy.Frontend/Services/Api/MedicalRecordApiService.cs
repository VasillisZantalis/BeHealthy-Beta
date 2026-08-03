using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.MedicalRecord;

namespace BeHealthy.Frontend.Services.Api;

public class MedicalRecordApiService : ApiClientBase, IMedicalRecordService
{
    public MedicalRecordApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<MedicalRecordResponse>> GetAllMedicalRecordsAsync()
        => await GetListAsync<MedicalRecordResponse>("medicalrecords");

    public async Task<MedicalRecordResponse?> GetMedicalRecordByIdAsync(int id)
        => await GetAsync<MedicalRecordResponse>($"medicalrecords/{id}");

    public async Task<IEnumerable<MedicalRecordResponse>> GetMedicalRecordsByPatientIdAsync(int patientId)
        => await GetListAsync<MedicalRecordResponse>($"medicalrecords/patient/{patientId}");

    public async Task AddMedicalRecordAsync(MedicalRecordCreateRequest medicalRecordDto)
        => await PostAsync("medicalrecords", medicalRecordDto);

    public async Task UpdateMedicalRecordAsync(MedicalRecordUpdateRequest medicalRecordDto)
        => await PutAsync("medicalrecords", medicalRecordDto);

    public async Task DeleteMedicalRecordAsync(int id)
        => await DeleteAsync($"medicalrecords/{id}");

    public async Task UpdateMedicalRecordNotesAsync(int id, string notes)
        => await PutAsync($"medicalrecords/{id}/notes", notes);
}
