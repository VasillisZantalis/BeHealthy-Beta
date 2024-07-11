using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos;
using BeHealthy.Shared.Models.Dtos.Prescription;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IMapper _mapper;

    public PrescriptionService(IPrescriptionRepository prescriptionRepository, IMapper mapper)
    {
        _prescriptionRepository = prescriptionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync()
    {
        var prescriptions = await _prescriptionRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
    }

    public async Task<PrescriptionDto> GetPrescriptionByIdAsync(int id)
    {
        var prescription = await _prescriptionRepository.GetByIdAsync(id);
        return _mapper.Map<PrescriptionDto>(prescription);
    }

    public async Task AddPrescriptionAsync(PrescriptionForCreationDto prescriptionDto)
    {
        var prescription = _mapper.Map<Prescription>(prescriptionDto);
        await _prescriptionRepository.AddAsync(prescription);
    }

    public async Task UpdatePrescriptionAsync(PrescriptionForUpdateDto prescriptionDto)
    {
        var prescription = _mapper.Map<Prescription>(prescriptionDto);
        await _prescriptionRepository.UpdateAsync(prescription);
    }

    public async Task DeletePrescriptionAsync(int id)
    {
        await _prescriptionRepository.DeleteAsync(id);
    }
}
