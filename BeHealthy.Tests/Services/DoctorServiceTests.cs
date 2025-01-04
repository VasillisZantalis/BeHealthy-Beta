using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Services;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;
using BeHealthy.Domain.Interfaces.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeHealthy.Tests.Services;

public class DoctorServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly DoctorService _doctorService;

    public DoctorServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockDoctorRepository = new Mock<IDoctorRepository>();

        _mockUnitOfWork.Setup(uow => uow.DoctorRepository).Returns(_mockDoctorRepository.Object);

        _doctorService = new DoctorService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetAllDoctorsAsync_ReturnsMappedDoctors()
    {
        // Arrange
        var doctors = new List<Doctor>
        {
            new Doctor
            {
                Id = 1,
                UserId = "1",
                FirstName = "John",
                LastName = "Doe",
                SpecialtyId = 1,
                DepartmentId = 1,
                CreatedAt = DateTime.Now,
                User = new ApplicationUser { PhoneNumber = "1234567890", Email = "john.doe@example.com" },
                Specialty = new Specialty { Name = "Cardiology" }
            },
            new Doctor
            {
                Id = 2,
                UserId = "2",
                FirstName = "John",
                LastName = "Does",
                SpecialtyId = 1,
                DepartmentId = 1,
                CreatedAt = DateTime.Now,
                User = new ApplicationUser { PhoneNumber = "1234567890", Email = "john.doe@example.com" },
                Specialty = new Specialty { Name = "Cardiology" }
            }
        };

        _mockDoctorRepository.Setup(repo => repo.GetAllDoctorsAsync()).ReturnsAsync(doctors);

        // Act
        var result = await _doctorService.GetAllDoctorsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<DoctorDto>>(result);
        Assert.Equal(doctors.Count, result.Count());
    }
}
