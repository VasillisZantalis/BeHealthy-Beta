using BeHealthy.Shared.Dtos.Visit;
using BeHealthy.Application.Interfaces;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class VisitService : IVisitService
{
    private readonly IVisitRepository _visitRepository;

    public VisitService(IVisitRepository visitRepository)
    {
        _visitRepository = visitRepository;
    }

    public async Task<IEnumerable<Visit>> GetAllVisitsAsync(CancellationToken cancellationToken = default)
    {
        return await _visitRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Visit?> GetVisitWithDetailsAsync(int visitId, CancellationToken cancellationToken = default)
    {
        return await _visitRepository.GetVisitWithDetailsAsync(visitId, cancellationToken);
    }

    public async Task<IEnumerable<Diagnosis>> GetDiagnosesByVisitIdAsync(int visitId, CancellationToken cancellationToken = default)
    {
        return await _visitRepository.GetDiagnosesByVisitIdAsync(visitId, cancellationToken);
    }

    public async Task<IEnumerable<Treatment>> GetTreatmentsByVisitIdAsync(int visitId, CancellationToken cancellationToken = default)
    {
        return await _visitRepository.GetTreatmentsByVisitIdAsync(visitId, cancellationToken);
    }

    public async Task<IEnumerable<LabResult>> GetLabResultsByVisitIdAsync(int visitId, CancellationToken cancellationToken = default)
    {
        return await _visitRepository.GetLabResultsByVisitIdAsync(visitId, cancellationToken);
    }

    public async Task<ServiceResponse> AddVisitAsync(VisitCreateRequest dto, CancellationToken cancellationToken = default)
    {
        var visit = dto.MapToDomain();
        await _visitRepository.AddAsync(visit, cancellationToken);
        await _visitRepository.SaveChangesAsync(cancellationToken);
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> UpdateVisitAsync(VisitUpdateRequest dto, CancellationToken cancellationToken = default)
    {
        var visit = await _visitRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (visit == null)
            return ServiceResponse.Failed(Resource.NotFound);

        dto.MapToEntity(visit);
        await _visitRepository.UpdateAsync(visit);
        await _visitRepository.SaveChangesAsync(cancellationToken);
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> DeleteVisitAsync(int id, CancellationToken cancellationToken = default)
    {
        if (!await _visitRepository.DeleteAsync(id, cancellationToken))
            return ServiceResponse.Failed(Resource.NotFound);

        await _visitRepository.SaveChangesAsync(cancellationToken);
        return ServiceResponse.Successful();
    }

    public async Task<IEnumerable<VisitResponse>> GetVisitsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var visits = await _visitRepository.GetVisitsByPatientIdAsync(patientId, cancellationToken);
        return visits.MapToDto();
    }
}
