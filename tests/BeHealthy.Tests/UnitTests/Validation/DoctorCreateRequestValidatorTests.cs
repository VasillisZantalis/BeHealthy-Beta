using BeHealthy.Shared.Common;
using BeHealthy.Tests.UnitTests.Fakes;
using BeHealthy.Validation.Doctors;
using FluentValidation.TestHelper;

namespace BeHealthy.Tests.UnitTests.Validation;

public class DoctorCreateRequestValidatorTests
{
    private static DoctorCreateRequest ValidRequest() => new()
    {
        FirstName = "Gregory",
        LastName = "House",
        Email = "house+md@ppth.org",
        Password = "Vic0din!",
        ConfirmPassword = "Vic0din!"
    };

    private static DoctorCreateRequestValidator CreateValidator(params string[] enabledSettings)
        => new(new FakeValidationSettingsProvider(enabledSettings));

    [Fact]
    public async Task Validate_ValidRequest_HasNoErrors()
    {
        // Act
        var result = await CreateValidator().TestValidateAsync(ValidRequest());

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_EmptyRequest_ReportsOneMessagePerRequiredField()
    {
        // Act
        var result = await CreateValidator().TestValidateAsync(new DoctorCreateRequest());

        // Assert: the rule chains stop at the first failure, so "required" is the only message per field.
        result.Errors.GroupBy(e => e.PropertyName).ShouldAllBe(g => g.Count() == 1);
        result.ShouldHaveValidationErrorFor(x => x.FirstName).WithErrorMessage(string.Format(Resource.PropertyRequired, Resource.FirstName));
        result.ShouldHaveValidationErrorFor(x => x.LastName).WithErrorMessage(string.Format(Resource.PropertyRequired, Resource.LastName));
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage(string.Format(Resource.PropertyRequired, Resource.Email));
        result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorMessage(string.Format(Resource.PropertyRequired, Resource.Password));
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword).WithErrorMessage(string.Format(Resource.PropertyRequired, Resource.ConfirmPassword));
    }

    [Fact]
    public async Task Validate_NameLongerThanTheColumn_HasError()
    {
        // Arrange
        var request = ValidRequest();
        request.FirstName = new string('a', 51);

        // Act
        var result = await CreateValidator().TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("@ppth.org")]
    public async Task Validate_InvalidEmail_HasError(string email)
    {
        // Arrange
        var request = ValidRequest();
        request.Email = email;

        // Act
        var result = await CreateValidator().TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage(Resource.InvalidEmailFormat);
    }

    [Theory]
    [InlineData("Ab1!", nameof(Resource.PasswordTooShort))]
    [InlineData("abcdef1!", nameof(Resource.PasswordNeedsUppercase))]
    [InlineData("ABCDEF1!", nameof(Resource.PasswordNeedsLowercase))]
    [InlineData("Abcdefg!", nameof(Resource.PasswordNeedsDigit))]
    [InlineData("Abcdefg1", nameof(Resource.PasswordNeedsNonAlphanumericCharacter))]
    public async Task Validate_WeakPassword_ReportsTheFirstBrokenRule(string password, string expectedResourceKey)
    {
        // Arrange
        var request = ValidRequest();
        request.Password = password;
        request.ConfirmPassword = password;
        var expected = expectedResourceKey == nameof(Resource.PasswordTooShort)
            ? string.Format(Resource.PasswordTooShort, 6)
            : (string)typeof(Resource).GetField(expectedResourceKey)!.GetValue(null)!;

        // Act
        var result = await CreateValidator().TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorMessage(expected).Only();
    }

    [Fact]
    public async Task Validate_PasswordsDoNotMatch_HasError()
    {
        // Arrange
        var request = ValidRequest();
        request.ConfirmPassword = "Different1!";

        // Act
        var result = await CreateValidator().TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword).WithErrorMessage(Resource.PasswordsDoNotMatch);
    }

    [Theory]
    [InlineData("12ab")]
    [InlineData("0123456")]
    public async Task Validate_InvalidPhoneNumber_HasError(string phoneNumber)
    {
        // Arrange
        var request = ValidRequest();
        request.PhoneNumber = phoneNumber;

        // Act
        var result = await CreateValidator().TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Fact]
    public async Task Validate_UnselectedOptionalDropdownSendsZero_HasError()
    {
        // Arrange: 0 is not a valid id; "no department" must be sent as null.
        var request = ValidRequest();
        request.DepartmentId = 0;

        // Act
        var result = await CreateValidator().TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DepartmentId);
    }

    [Fact]
    public async Task Validate_NoSpecialtyAndSettingOff_HasNoError()
    {
        // Act
        var result = await CreateValidator().TestValidateAsync(ValidRequest());

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.SpecialtyId);
    }

    [Fact]
    public async Task Validate_NoSpecialtyAndSettingOn_HasError()
    {
        // Arrange
        var validator = CreateValidator(AppSettingKeys.DoNotAllowDoctorWithoutSpecialty);

        // Act
        var result = await validator.TestValidateAsync(ValidRequest());

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SpecialtyId)
            .WithErrorMessage(string.Format(Resource.PropertyRequired, Resource.Specialty));
    }

    [Fact]
    public async Task Validate_SpecialtySelectedAndSettingOn_HasNoError()
    {
        // Arrange
        var validator = CreateValidator(AppSettingKeys.DoNotAllowDoctorWithoutSpecialty);
        var request = ValidRequest();
        request.SpecialtyId = 3;

        // Act
        var result = await validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.SpecialtyId);
    }
}
