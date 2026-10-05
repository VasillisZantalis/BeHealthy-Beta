using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.MedicalRecord;

namespace BeHealthy.Application.Services.Interfaces;

public interface IMedicalRecordService
{
    Task<IEnumerable<MedicalRecordResponse>> GetAllMedicalRecordsAsync(CancellationToken cancellationToken = default);
    Task<MedicalRecordResponse?> GetMedicalRecordByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MedicalRecordResponse>> GetMedicalRecordsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task AddMedicalRecordAsync(MedicalRecordCreateRequest medicalRecordDto, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateMedicalRecordAsync(MedicalRecordUpdateRequest medicalRecordDto, CancellationToken cancellationToken = default);
    Task DeleteMedicalRecordAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateMedicalRecordNotesAsync(int id, string? notes, CancellationToken cancellationToken = default);
}
