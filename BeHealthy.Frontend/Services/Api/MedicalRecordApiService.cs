using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.MedicalRecord;

namespace BeHealthy.Frontend.Services.Api;

public class MedicalRecordApiService : ApiClientBase, IMedicalRecordService
{
    public MedicalRecordApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<MedicalRecordDto>> GetAllMedicalRecordsAsync()
        => await GetListAsync<MedicalRecordDto>("medicalrecords");

    public async Task<MedicalRecordDto?> GetMedicalRecordByIdAsync(int id)
        => await GetAsync<MedicalRecordDto>($"medicalrecords/{id}");

    public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByPatientIdAsync(int patientId)
        => await GetListAsync<MedicalRecordDto>($"medicalrecords/patient/{patientId}");

    public async Task AddMedicalRecordAsync(MedicalRecordCreateDto medicalRecordDto)
        => await PostAsync("medicalrecords", medicalRecordDto);

    public async Task UpdateMedicalRecordAsync(MedicalRecordUpdateDto medicalRecordDto)
        => await PutAsync("medicalrecords", medicalRecordDto);

    public async Task DeleteMedicalRecordAsync(int id)
        => await DeleteAsync($"medicalrecords/{id}");

    public async Task UpdateMedicalRecordNotesAsync(int id, string notes)
        => await PutAsync($"medicalrecords/{id}/notes", notes);
}
