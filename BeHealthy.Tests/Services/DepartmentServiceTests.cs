using AutoFixture;
using BeHealthy.Application.Dtos.Department;
using BeHealthy.Application.Services;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Shared.Locales;
using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace BeHealthy.Tests.Services;

public class DepartmentServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IDepartmentRepository> _mockDepartmentRepository;
    private readonly IDepartmentService _sut;
    private readonly IFixture _fixture;

    public DepartmentServiceTests()
    {
        _fixture = new Fixture();

        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockDepartmentRepository = new Mock<IDepartmentRepository>();

        _mockUnitOfWork.Setup(uow => uow.DepartmentRepository).Returns(_mockDepartmentRepository.Object);

        _sut = new DepartmentService(_mockUnitOfWork.Object);
    }

    #region AddDepartmentAsync
    
    [Fact]
    public async Task AddDepartmentAsync_ValidDepartment_CreatesDepartment()
    {
        //Arrange
        DepartmentForCreationDto departmentForCreationDto = _fixture.Create<DepartmentForCreationDto>();

        _mockUnitOfWork.Setup(uow => uow.DepartmentRepository.AddAsync(It.IsAny<Department>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.AddDepartmentAsync(departmentForCreationDto);

        //Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeTrue();
        result.ErrorMessage.ShouldBeNull();
    }

    [Fact]
    public async Task AddDepartmentAsync_NullDepartment_ReturnsFailedResponse()
    {
        //Arrange
        _mockUnitOfWork.Setup(uow => uow.DepartmentRepository.AddAsync(It.IsAny<Department>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.AddDepartmentAsync(null!);

        //Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldNotBeNull();
    }

    [Fact]
    public async Task AddDepartmentAsync_ValidHeadOfDepartmentId_CreatesDepartment()
    {
        // Arrange
        DepartmentForCreationDto departmentForCreationDto = _fixture.Build<DepartmentForCreationDto>()
            .With(w => w.HeadOfDepartmentId, 1)
            .Create();

        _mockUnitOfWork.Setup(uow => uow.DepartmentRepository.AddAsync(It.IsAny<Department>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.AddDepartmentAsync(departmentForCreationDto);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeTrue();
        result.ErrorMessage.ShouldBeNull();
        result.ErrorMessage.ShouldNotBe(Resource.SomethingWentWrong);
    }

    [Fact]
    public async Task AddDepartmentAsync_InvalidHeadOfDepartmentId_ReturnsFailedResponse()
    {
        // Arrange
        DepartmentForCreationDto departmentForCreationDto = _fixture.Build<DepartmentForCreationDto>()
            .With(w => w.HeadOfDepartmentId, 99999)
            .Create();

        _mockUnitOfWork.Setup(uow => uow.DepartmentRepository.AddAsync(It.IsAny<Department>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        var result = await _sut.AddDepartmentAsync(departmentForCreationDto);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe(Resource.SomethingWentWrong);
    }

    #endregion
}
