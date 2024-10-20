using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
    {
        var doctors = await _unitOfWork.DoctorRepository.GetAllDoctorsAsync();
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public async Task<DoctorDto> GetDoctorByIdAsync(int id)
    {
        var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id);
        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task AddDoctorAsync(DoctorForCreationDto doctorDto)
    {
        var doctor = _mapper.Map<Doctor>(doctorDto);
        await _unitOfWork.DoctorRepository.AddAsync(doctor);
    }

    public async Task UpdateDoctorAsync(int id, DoctorForUpdateDto doctorDto)
    {
        var doctor = _mapper.Map<Doctor>(doctorDto);
        await _unitOfWork.DoctorRepository.UpdateAsync(doctor);
    }

    public async Task DeleteDoctorAsync(int id)
    {
        await _unitOfWork.DoctorRepository.DeleteDoctorAsync(id);
    }

    public async Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsByUserIdAsync(string userId)
    {
        var doctorAppointments = await _unitOfWork.DoctorRepository.GetDoctorAppointmentsByUserIdAsync(userId);

        return _mapper.Map<IEnumerable<AppointmentDto>>(doctorAppointments);
    }
}

