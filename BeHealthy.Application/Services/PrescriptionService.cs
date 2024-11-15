using AutoMapper;
using BeHealthy.Application.Dtos.Prescription;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;

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

    public async Task AddPrescriptionAsync(PrescriptionForCreationDto prescriptionDto)
    {
        var prescription = _mapper.Map<Prescription>(prescriptionDto);
        await _unitOfWork.PrescriptionRepository.AddAsync(prescription);
    }

    public async Task UpdatePrescriptionAsync(PrescriptionForUpdateDto prescriptionDto)
    {
        var prescription = _mapper.Map<Prescription>(prescriptionDto);
        await _unitOfWork.PrescriptionRepository.UpdateAsync(prescription);
    }

    public async Task DeletePrescriptionAsync(int id)
    {
        await _unitOfWork.PrescriptionRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByPatientIdAsync(int id)
    {
        var prescriptions = await _unitOfWork.PrescriptionRepository.GetPrescriptionsByPatientIdAsync(id);

        return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
    }
}
