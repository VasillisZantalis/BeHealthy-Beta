using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Prescription;
using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IUnitOfWork _unitOfWork;

    public PrescriptionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync()
    {
        var prescriptions = await _unitOfWork.PrescriptionRepository.GetAllAsync();
        return prescriptions.MapToDto();
    }

    public async Task<PrescriptionDto?> GetPrescriptionByIdAsync(int id)
    {
        var prescription = await _unitOfWork.PrescriptionRepository.GetByIdAsync(id);
        return prescription?.MapToDto();
    }

    public async Task<ServiceResponse> AddPrescriptionAsync(PrescriptionCreateDto prescriptionDto)
    {
        try
        {
            var prescription = prescriptionDto.MapToDomain();
            await _unitOfWork.PrescriptionRepository.AddAsync(prescription);

            return prescription.Id > 0 ? ServiceResponse.Successful() : ServiceResponse.Failed();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed();
        }
        
    }

    public async Task<ServiceResponse> UpdatePrescriptionAsync(PrescriptionUpdateDto prescriptionDto)
    {
        try
        {
            var existingPrescr = await _unitOfWork.PrescriptionRepository.GetByIdAsync(prescriptionDto.Id);

            if (existingPrescr is null)
            {
                var errorMessage = string.Join(" ", Resource.NotFound, Resource.Prescription);
                return ServiceResponse.Failed(errorMessage);
            }

            var updatedPrescription = prescriptionDto.MapToDomain();

            updatedPrescription.Id = existingPrescr.Id;
            updatedPrescription.DoctorId = existingPrescr.DoctorId;
            updatedPrescription.PatientId = existingPrescr.PatientId;
            updatedPrescription.DatePrescribed = existingPrescr.DatePrescribed;

            await _unitOfWork.PrescriptionRepository.UpdateAsync(updatedPrescription);

            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed();
        }
        
    }

    public async Task<ServiceResponse> DeletePrescriptionAsync(int id)
    {
        await _unitOfWork.PrescriptionRepository.DeleteAsync(id);

        return ServiceResponse.Successful();
    }

    public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByPatientIdAsync(int id)
    {
        var prescriptions = await _unitOfWork.PrescriptionRepository.GetPrescriptionsByPatientIdAsync(id);

        return prescriptions.MapToDto();
    }
}
