using BeHealthy.Shared.Dtos.Department;
using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Tests.UnitTests.Services;

public class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> _mockDepartmentRepository;
    private readonly IDepartmentService _sut;
    private readonly IFixture _fixture;

    public DepartmentServiceTests()
    {
        _fixture = new Fixture();

        _mockDepartmentRepository = new Mock<IDepartmentRepository>();

        _sut = new DepartmentService(
            _mockDepartmentRepository.Object,
            Mock.Of<IDoctorRepository>(),
            Mock.Of<INurseRepository>(),
            Mock.Of<IPatientRepository>(),
            Mock.Of<IRoomRepository>());
    }

    #region AddDepartmentAsync

    [Fact]
    public async Task AddDepartmentAsync_ValidDepartment_CreatesDepartment()
    {
        //Arrange
        DepartmentCreateRequest departmentForCreationDto = _fixture.Create<DepartmentCreateRequest>();

        _mockDepartmentRepository.Setup(r => r.AddAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()))
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
        _mockDepartmentRepository.Setup(r => r.AddAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()))
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
        DepartmentCreateRequest departmentForCreationDto = _fixture.Build<DepartmentCreateRequest>()
            .With(w => w.HeadOfDepartmentId, 1)
            .Create();

        _mockDepartmentRepository.Setup(r => r.AddAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()))
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
        DepartmentCreateRequest departmentForCreationDto = _fixture.Build<DepartmentCreateRequest>()
            .With(w => w.HeadOfDepartmentId, 99999)
            .Create();

        _mockDepartmentRepository.Setup(r => r.AddAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        var result = await _sut.AddDepartmentAsync(departmentForCreationDto);

        // Assert
        result.Success.ShouldBeFalse();
    }

    #endregion
}
