using BeHealthy.Shared.Parameters;
using BeHealthy.Validation.Parameters;
using FluentValidation.TestHelper;

namespace BeHealthy.Tests.UnitTests.Validation;

public class QueryParametersValidatorTests
{
    private readonly DoctorQueryParametersValidator _sut = new();

    [Fact]
    public void Validate_Defaults_HasNoErrors()
    {
        // Act
        var result = _sut.TestValidate(new DoctorQueryParameters());

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_PageNumberBelowOne_HasError(int pageNumber)
    {
        // Act
        var result = _sut.TestValidate(new DoctorQueryParameters { PageNumber = pageNumber });

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Validate_PageSizeOutOfRange_HasError(int pageSize)
    {
        // Act
        var result = _sut.TestValidate(new DoctorQueryParameters { PageSize = pageSize });

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Validate_LargestPageSizeTheUiOffers_HasNoError()
    {
        // Act
        var result = _sut.TestValidate(new DoctorQueryParameters { PageSize = 100 });

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
    }
}
