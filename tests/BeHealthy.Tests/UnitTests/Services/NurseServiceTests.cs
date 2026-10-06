using BeHealthy.Shared.Parameters;
using BeHealthy.Tests.UnitTests.Fakes;

namespace BeHealthy.Tests.UnitTests.Services;

public class NurseServiceTests
{
    private readonly Mock<INurseRepository> _mockNurseRepository;
    private readonly NurseService _sut;

    public NurseServiceTests()
    {
        _mockNurseRepository = new Mock<INurseRepository>();

        _sut = new NurseService(
            _mockNurseRepository.Object,
            Mock.Of<IPatientRepository>(),
            Mock.Of<IAppointmentRepository>(),
            Mock.Of<IUserService>(),
            new FakeTransactionManager());
    }

    #region GetAllNursesAsync

    [Fact]
    public async Task GetAllNursesAsync_WithSearchTerm_CountsOnlyMatchingNurses()
    {
        // Arrange
        Expression<Func<Nurse, bool>>? countPredicate = null;
        _mockNurseRepository
            .Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Nurse, bool>>>(), It.IsAny<CancellationToken>()))
            .Callback<Expression<Func<Nurse, bool>>, CancellationToken>((p, _) => countPredicate = p)
            .ReturnsAsync(1);

        var nurses = new[]
        {
            new Nurse { FirstName = "Maria", LastName = "Lopez" },
            new Nurse { FirstName = "Eva", LastName = "Brown" },
        };

        // Act
        var result = await _sut.GetAllNursesAsync(new QueryParameters { SearchTerm = "Maria" });

        // Assert
        result.TotalCount.ShouldBe(1);
        _mockNurseRepository.Verify(r => r.GetCountAsync(It.IsAny<CancellationToken>()), Times.Never);
        countPredicate.ShouldNotBeNull();
        nurses.Count(countPredicate.Compile()).ShouldBe(1);
    }

    #endregion
}
