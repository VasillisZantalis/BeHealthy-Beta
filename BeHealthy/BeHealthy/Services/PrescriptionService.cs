using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Prescription;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

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
}
