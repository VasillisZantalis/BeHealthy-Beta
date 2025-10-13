using BeHealthy.Application.Dtos.Department;
using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Tests.UnitTests.Services;

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
        DepartmentCreateDto departmentForCreationDto = _fixture.Create<DepartmentCreateDto>();

        _mockUnitOfWork.Setup(uow => uow.DepartmentRepository.AddAsync(It.IsAny<Department>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.AddDepartmentAsync(departmentForCreationDto);

        //Assert
        result.Success.ShouldBeTrue();
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
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task AddDepartmentAsync_ValidHeadOfDepartmentId_CreatesDepartment()
    {
        // Arrange
        DepartmentCreateDto departmentForCreationDto = _fixture.Build<DepartmentCreateDto>()
            .With(w => w.HeadOfDepartmentId, 1)
            .Create();

        _mockUnitOfWork.Setup(uow => uow.DepartmentRepository.AddAsync(It.IsAny<Department>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.AddDepartmentAsync(departmentForCreationDto);

        // Assert
        result.Success.ShouldBeTrue();
    }

    [Fact]
    public async Task AddDepartmentAsync_InvalidHeadOfDepartmentId_ReturnsFailedResponse()
    {
        // Arrange
        DepartmentCreateDto departmentForCreationDto = _fixture.Build<DepartmentCreateDto>()
            .With(w => w.HeadOfDepartmentId, 99999)
            .Create();

        _mockUnitOfWork.Setup(uow => uow.DepartmentRepository.AddAsync(It.IsAny<Department>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        var result = await _sut.AddDepartmentAsync(departmentForCreationDto);

        // Assert
        result.Success.ShouldBeFalse();
    }

    #endregion
}
