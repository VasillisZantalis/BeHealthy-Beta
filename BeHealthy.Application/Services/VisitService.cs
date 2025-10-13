using BeHealthy.Application.Dtos.Visit;
using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class VisitService : IVisitService
{
    private readonly IVisitRepository _visitRepository;

    public VisitService(IVisitRepository visitRepository)
    {
        _visitRepository = visitRepository;
    }

    public async Task<IEnumerable<Visit>> GetAllVisitsAsync()
    {
        return await _visitRepository.GetAllAsync();
    }

    public async Task<Visit?> GetVisitWithDetailsAsync(int visitId)
    {
        return await _visitRepository.GetVisitWithDetailsAsync(visitId);
    }

    public async Task<IEnumerable<Diagnosis>> GetDiagnosesByVisitIdAsync(int visitId)
    {
        return await _visitRepository.GetDiagnosesByVisitIdAsync(visitId);
    }

    public async Task<IEnumerable<Treatment>> GetTreatmentsByVisitIdAsync(int visitId)
    {
        return await _visitRepository.GetTreatmentsByVisitIdAsync(visitId);
    }

    public async Task<IEnumerable<LabResult>> GetLabResultsByVisitIdAsync(int visitId)
    {
        return await _visitRepository.GetLabResultsByVisitIdAsync(visitId);
    }

    public async Task<ServiceResponse> AddVisitAsync(VisitCreateDto dto)
    {
        var visit = dto.MapToDomain();
        await _visitRepository.AddAsync(visit);
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> UpdateVisitAsync(VisitUpdateDto dto)
    {
        var visit = await _visitRepository.GetByIdAsync(dto.Id);
        if (visit == null)
            return ServiceResponse.Failed(Resource.NotFound);

        dto.MapToEntity(visit);
        await _visitRepository.UpdateAsync(visit);
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> DeleteVisitAsync(int id)
    {
        await _visitRepository.DeleteAsync(id);
        return ServiceResponse.Successful();
    }

    public async Task<IEnumerable<VisitDto>> GetVisitsByPatientIdAsync(int patientId)
    {
        var visits = await _visitRepository.GetVisitsByPatientIdAsync(patientId);
        return visits.MapToDto();
    }
}