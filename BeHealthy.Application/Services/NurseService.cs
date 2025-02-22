using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Dtos.Nurse;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;

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

    public async Task<NurseDto?> GetNurseByIdAsync(int id)
    {
        var nurse = await _unitOfWork.NurseRepository.GetByIdAsync(id);
        return nurse?.MapToDto();
    }

    public async Task<ServiceResponse> AddNurseAsync(NurseForCreationDto nurseDto)
    {
        try
        {
            var nurse = nurseDto.MapToDomain();
            await _unitOfWork.NurseRepository.AddAsync(nurse);
            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed();
        }
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

    public async Task<IEnumerable<NurseDto>> GetNursesOfPatientByUserId(string userId)
    {
        List<Nurse> nurses = new();

        var patient = await _unitOfWork.PatientRepository.GetByUserIdAsync(userId);

        if (patient is null)
            return Enumerable.Empty<NurseDto>();

        var patientAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByPatientIdAsync(patient.Id);

        List<int?> nurseIds = patientAppointments
            .Select(s => s.NurseId)
            .Distinct()
            .ToList();

        if (nurseIds.Any())
        {
            var nursesThatTreatPatient = await _unitOfWork.NurseRepository.FindAsync(w => nurseIds.Contains(w.Id));
            nurses.AddRange(nursesThatTreatPatient);
        }

        var distinctNurses = nurses
            .GroupBy(g => g.Id)
            .Select(s => s.First())
            .ToList();

        return distinctNurses.MapToDto();
    }
}


