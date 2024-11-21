using BeHealthy.Application.Dtos.Nurse;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Application.Mappings;

namespace BeHealthy.Application.Services;

public class NurseService : INurseService
{
    private readonly IUnitOfWork _unitOfWork;

    public NurseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<NurseDto>> GetAllNursesAsync()
    {
        var nurses = await _unitOfWork.NurseRepository.GetAllNursesAsync();
        return nurses.MapToDto();
    }

    public async Task<NurseDto> GetNurseByIdAsync(int id)
    {
        var nurse = await _unitOfWork.NurseRepository.GetByIdAsync(id);
        return nurse.MapToDto();
    }

    public async Task AddNurseAsync(NurseForCreationDto nurseDto)
    {
        var nurse = nurseDto.MapToDomain();
        await _unitOfWork.NurseRepository.AddAsync(nurse);
    }

    public async Task UpdateNurseAsync(int id, NurseForUpdateDto nurseDto)
    {
        var nurse = nurseDto.MapToDomain();
        await _unitOfWork.NurseRepository.UpdateAsync(nurse);
    }

    public async Task DeleteNurseAsync(int id)
    {
        await _unitOfWork.NurseRepository.DeleteNurseAsync(id);
    }
}


