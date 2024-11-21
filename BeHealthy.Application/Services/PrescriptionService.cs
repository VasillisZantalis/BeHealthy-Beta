using AutoMapper;
using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Dtos.Prescription;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PrescriptionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync()
    {
        var prescriptions = await _unitOfWork.PrescriptionRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
    }

    public async Task<PrescriptionDto> GetPrescriptionByIdAsync(int id)
    {
        var prescription = await _unitOfWork.PrescriptionRepository.GetByIdAsync(id);
        return _mapper.Map<PrescriptionDto>(prescription);
    }

    public async Task<ServiceResponse> AddPrescriptionAsync(PrescriptionForCreationDto prescriptionDto)
    {
        var prescription = _mapper.Map<Prescription>(prescriptionDto);
        await _unitOfWork.PrescriptionRepository.AddAsync(prescription);

        return prescription.Id > 0 ? ServiceResponse.Successful() : ServiceResponse.Failed();
    }

    public async Task<ServiceResponse> UpdatePrescriptionAsync(PrescriptionForUpdateDto prescriptionDto)
    {
        var existingPrescr = await _unitOfWork.PrescriptionRepository.GetByIdAsync(prescriptionDto.Id);

        if (existingPrescr is null)
        {
            var errorMessage = string.Join(" ", Resource.NotFound, Resource.Prescription);
            return ServiceResponse.Failed(errorMessage);
        }

        var updatedPrescription = _mapper.Map<Prescription>(prescriptionDto);

        updatedPrescription.Id = existingPrescr.Id;
        updatedPrescription.DoctorId = existingPrescr.DoctorId;
        updatedPrescription.PatientId = existingPrescr.PatientId;
        updatedPrescription.DatePrescribed = existingPrescr.DatePrescribed;

        await _unitOfWork.PrescriptionRepository.UpdateAsync(updatedPrescription);

        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> DeletePrescriptionAsync(int id)
    {
        await _unitOfWork.PrescriptionRepository.DeleteAsync(id);

        return ServiceResponse.Successful();
    }

    public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByPatientIdAsync(int id)
    {
        var prescriptions = await _unitOfWork.PrescriptionRepository.GetPrescriptionsByPatientIdAsync(id);

        return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
    }
}
