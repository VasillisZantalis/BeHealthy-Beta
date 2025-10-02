using BeHealthy.Domain.Entities;
using BeHealthy.Application.Dtos.Allergy;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Application.Services;

public class AllergyService : IAllergyService
{
    private readonly IAllergyRepository _allergyRepository;

    public AllergyService(IAllergyRepository allergyRepository)
    {
        _allergyRepository = allergyRepository;
    }

    public async Task<IEnumerable<Allergy>> GetAllergiesByPatientIdAsync(int patientId)
    {
        return await _allergyRepository.GetAllergiesByPatientIdAsync(patientId);
    }

    public async Task<ServiceResponse> AddAllergyAsync(AllergyCreateDto dto)
    {
        var allergy = dto.ToEntity();
        await _allergyRepository.AddAsync(allergy);
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> UpdateAllergyAsync(AllergyUpdateDto dto)
    {
        var allergy = await _allergyRepository.GetByIdAsync(dto.Id);
        if (allergy == null)
            return ServiceResponse.Failed("Allergy not found.");

        dto.MapToEntity(allergy);
        await _allergyRepository.UpdateAsync(allergy);
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> DeleteAllergyAsync(int id)
    {
        await _allergyRepository.DeleteAsync(id);
        return ServiceResponse.Successful();
    }
}