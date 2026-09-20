using BeHealthy.Shared.Dtos.Visit;
using BeHealthy.Application.Interfaces;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class VisitService : IVisitService
{
    private readonly IUnitOfWork _unitOfWork;

    public VisitService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Visit>> GetAllVisitsAsync()
    {
        return await _unitOfWork.VisitRepository.GetAllAsync();
    }

    public async Task<Visit?> GetVisitWithDetailsAsync(int visitId)
    {
        return await _unitOfWork.VisitRepository.GetVisitWithDetailsAsync(visitId);
    }

    public async Task<IEnumerable<Diagnosis>> GetDiagnosesByVisitIdAsync(int visitId)
    {
        return await _unitOfWork.VisitRepository.GetDiagnosesByVisitIdAsync(visitId);
    }

    public async Task<IEnumerable<Treatment>> GetTreatmentsByVisitIdAsync(int visitId)
    {
        return await _unitOfWork.VisitRepository.GetTreatmentsByVisitIdAsync(visitId);
    }

    public async Task<IEnumerable<LabResult>> GetLabResultsByVisitIdAsync(int visitId)
    {
        return await _unitOfWork.VisitRepository.GetLabResultsByVisitIdAsync(visitId);
    }

    public async Task<ServiceResponse> AddVisitAsync(VisitCreateRequest dto)
    {
        var visit = dto.MapToDomain();
        await _unitOfWork.VisitRepository.AddAsync(visit);
        await _unitOfWork.SaveChangesAsync();
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> UpdateVisitAsync(VisitUpdateRequest dto)
    {
        var visit = await _unitOfWork.VisitRepository.GetByIdAsync(dto.Id);
        if (visit == null)
            return ServiceResponse.Failed(Resource.NotFound);

        dto.MapToEntity(visit);
        await _unitOfWork.VisitRepository.UpdateAsync(visit);
        await _unitOfWork.SaveChangesAsync();
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> DeleteVisitAsync(int id)
    {
        await _unitOfWork.VisitRepository.DeleteAsync(id);
        return ServiceResponse.Successful();
    }

    public async Task<IEnumerable<VisitResponse>> GetVisitsByPatientIdAsync(int patientId)
    {
        var visits = await _unitOfWork.VisitRepository.GetVisitsByPatientIdAsync(patientId);
        return visits.MapToDto();
    }
}
