using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.MedicalRecord;
using BeHealthy.Shared.Models.Dtos.Nurse;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

public class NurseService : INurseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public NurseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Nurse>> GetAllNursesAsync()
    {
        var nurses = await _unitOfWork.NurseRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<Nurse>>(nurses);
    }

    public async Task<Nurse> GetNurseByIdAsync(int id)
    {
        var nurse = await _unitOfWork.NurseRepository.GetByIdAsync(id);
        return _mapper.Map<Nurse>(nurse);
    }

    public async Task AddNurseAsync(NurseForCreationDto nurseDto)
    {
        var nurse = _mapper.Map<Nurse>(nurseDto);
        await _unitOfWork.NurseRepository.AddAsync(nurse);
    }

    public async Task UpdateNurseAsync(NurseForUpdateDto nurseDto)
    {
        var nurse = _mapper.Map<Nurse>(nurseDto);
        await _unitOfWork.NurseRepository.UpdateAsync(nurse);
    }

    public async Task DeleteNurseAsync(int id)
    {
        await _unitOfWork.NurseRepository.DeleteAsync(id);
    }
}


