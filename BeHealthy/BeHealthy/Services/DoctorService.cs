using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMapper _mapper;

    public DoctorService(IDoctorRepository doctorRepository, IMapper mapper)
    {
        _doctorRepository = doctorRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
    {
        var doctors = await _doctorRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public async Task<DoctorDto> GetDoctorByIdAsync(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);
        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task AddDoctorAsync(DoctorForCreationDto doctorDto)
    {
        var doctor = _mapper.Map<Doctor>(doctorDto);
        await _doctorRepository.AddAsync(doctor);
    }

    public async Task UpdateDoctorAsync(DoctorForUpdateDto doctorDto)
    {
        var doctor = _mapper.Map<Doctor>(doctorDto);
        await _doctorRepository.UpdateAsync(doctor);
    }

    public async Task DeleteDoctorAsync(int id)
    {
        await _doctorRepository.DeleteAsync(id);
    }

    public async Task<List<DoctorDto>> GetDummyDoctors()
    {
        await Task.Delay(1000);

        return new List<DoctorDto>
        {
            new DoctorDto
            {
                Id = 1,
                UserId = "user1-id",
                FirstName = "John",
                LastName = "Doe",
                Specialty = "Cardiology",
                PhoneNumber = "123-456-7890"
            },
            new DoctorDto
            {
                Id = 2,
                UserId = "user2-id",
                FirstName = "Jane",
                LastName = "Smith",
                Specialty = "Neurology",
                PhoneNumber = "123-456-7891"
            },
            new DoctorDto
            {
                Id = 3,
                UserId = "user3-id",
                FirstName = "Alice",
                LastName = "Brown",
                Specialty = "Pediatrics",
                PhoneNumber = "123-456-7892"
            },
            new DoctorDto
            {
                Id = 4,
                UserId = "user4-id",
                FirstName = "Michael",
                LastName = "Johnson",
                Specialty = "Orthopedics",
                PhoneNumber = "123-456-7893"
            },
            new DoctorDto
            {
                Id = 5,
                UserId = "user5-id",
                FirstName = "Emily",
                LastName = "Davis",
                Specialty = "Dermatology",
                PhoneNumber = "123-456-7894"
            }
        };
    }
}

