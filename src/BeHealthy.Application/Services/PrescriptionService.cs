using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Prescription;
using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _prescriptionRepository;

    public PrescriptionService(IPrescriptionRepository prescriptionRepository)
    {
        _prescriptionRepository = prescriptionRepository;
    }

    public async Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync(CancellationToken cancellationToken = default)
    {
        var prescriptions = await _prescriptionRepository.GetAllAsync(cancellationToken);
        return prescriptions.MapToDto();
    }

    public async Task<PrescriptionResponse?> GetPrescriptionByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var prescription = await _prescriptionRepository.GetByIdAsync(id, cancellationToken);
        return prescription?.MapToDto();
    }

    public async Task<ServiceResponse> AddPrescriptionAsync(PrescriptionCreateRequest prescriptionDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var prescription = prescriptionDto.MapToDomain();
            await _prescriptionRepository.AddAsync(prescription, cancellationToken);
            await _prescriptionRepository.SaveChangesAsync(cancellationToken);

            return prescription.Id > 0 ? ServiceResponse.Successful() : ServiceResponse.Failed();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed();
        }

    }

    public async Task<ServiceResponse> UpdatePrescriptionAsync(PrescriptionUpdateRequest prescriptionDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingPrescr = await _prescriptionRepository.GetByIdAsync(prescriptionDto.Id, cancellationToken);

            if (existingPrescr is null)
            {
                var errorMessage = string.Join(" ", Resource.NotFound, Resource.Prescription);
                return ServiceResponse.Failed(errorMessage);
            }

            existingPrescr.Medication = prescriptionDto.Medication;
            existingPrescr.Dosage = prescriptionDto.Dosage;

            await _prescriptionRepository.UpdateAsync(existingPrescr);
            await _prescriptionRepository.SaveChangesAsync(cancellationToken);

            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed();
        }

    }

    public async Task<ServiceResponse> DeletePrescriptionAsync(int id, CancellationToken cancellationToken = default)
    {
        if (!await _prescriptionRepository.DeleteAsync(id, cancellationToken))
        {
            return ServiceResponse.Failed(Resource.NotFound);
        }

        await _prescriptionRepository.SaveChangesAsync(cancellationToken);
        return ServiceResponse.Successful();
    }

    public async Task<IEnumerable<PrescriptionResponse>> GetPrescriptionsByPatientIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var prescriptions = await _prescriptionRepository.GetPrescriptionsByPatientIdAsync(id, cancellationToken);

        return prescriptions.MapToDto();
    }
}
