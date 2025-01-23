using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Services;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Components.Pages.Doctors;
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
    private readonly IDoctorService _sut;

    public DoctorServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockDoctorRepository = new Mock<IDoctorRepository>();

        _mockUnitOfWork.Setup(uow => uow.DoctorRepository).Returns(_mockDoctorRepository.Object);

        _sut = new DoctorService(_mockUnitOfWork.Object);
    }

    #region GetAllDoctorsAsync

    [Fact]
    public async Task GetAllDoctorsAsync_ListFilled_ReturnsListDoctorDto()
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

        _mockUnitOfWork.Setup(uow => uow.DoctorRepository.GetAllDoctorsAsync()).ReturnsAsync(doctors);

        // Act
        var result = await _sut.GetAllDoctorsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<DoctorDto>>(result);
        Assert.Equal(doctors.Count, result.Count());
    }

    [Fact]
    public async Task GetAllDoctorsAsync_EmptyList_ReturnsEmptyListOfDoctorDto()
    {
        //Arrange
        _mockDoctorRepository
            .Setup(repo => repo.GetAllDoctorsAsync())
            .ReturnsAsync(new List<Doctor>());

        //Act
        var result = await _sut.GetAllDoctorsAsync();

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllDoctorsAsync_RepositoryCalledOnce()
    {
        //Arrange
        _mockDoctorRepository
            .Setup(repo => repo.GetAllDoctorsAsync())
            .ReturnsAsync(new List<Doctor>());

        //Act
        await _sut.GetAllDoctorsAsync();

        //Assert
        _mockDoctorRepository.Verify(repo => repo.GetAllDoctorsAsync(), Times.Once);
    }

    #endregion

    #region GetDoctorByIdAsync

    [Fact]
    public async Task GetDoctorByIdAsync_ValidId_ReturnsMappedDoctor()
    {
        // Arrange
        var doctorId = 1;
        var doctor = new Doctor
        {
            Id = doctorId,
            UserId = "1",
            FirstName = "John",
            LastName = "Doe",
            SpecialtyId = 1,
            DepartmentId = 1,
            CreatedAt = DateTime.Now,
            User = new ApplicationUser { PhoneNumber = "1234567890", Email = "john.doe@example.com" },
            Specialty = new Specialty { Name = "Cardiology" }
        };

        _mockUnitOfWork.Setup(uow => uow.DoctorRepository.GetByIdAsync(doctorId)).ReturnsAsync(doctor);

        // Act
        var result = await _sut.GetDoctorByIdAsync(doctorId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<DoctorDto>(result);
        Assert.Equal(doctor.Id, result.Id);
        Assert.Equal(doctor.FirstName, result.FirstName);
        Assert.Equal(doctor.LastName, result.LastName);
        Assert.Equal(doctor.User.Email, result.Email);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        _mockUnitOfWork.Setup(uow => uow.DoctorRepository.GetByIdAsync(It.IsAny<int>()))
                       .ReturnsAsync(() => null);

        // Act
        var result = await _sut.GetDoctorByIdAsync(9999);

        // Assert
        Assert.Null(result);
    }

    #endregion
}
