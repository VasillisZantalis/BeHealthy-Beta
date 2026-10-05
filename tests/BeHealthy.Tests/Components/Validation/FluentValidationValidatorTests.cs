using Blazored.FluentValidation;
using BeHealthy.Shared.Common;
using BeHealthy.Tests.UnitTests.Fakes;
using BeHealthy.Validation;
using BeHealthy.Validation.Settings;
using Microsoft.AspNetCore.Components.Forms;

namespace BeHealthy.Tests.Components.Validation;

/// <summary>
/// The front end validates with Blazored's &lt;FluentValidationValidator /&gt;, which resolves the
/// shared validators from DI. Blazored.FluentValidation 2.2 is built against FluentValidation 11,
/// while the solution uses 12; these tests exercise that combination, including the async,
/// settings-driven rules.
/// </summary>
public class FluentValidationValidatorTests : TestContext
{
    private FluentValidationValidator? _validator;

    private IRenderedComponent<EditForm> RenderForm(object model)
        => RenderComponent<EditForm>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.ChildContent, (EditContext _) => builder =>
            {
                builder.OpenComponent<FluentValidationValidator>(0);
                builder.AddComponentReferenceCapture(1, component => _validator = (FluentValidationValidator)component);
                builder.CloseComponent();
                builder.OpenComponent<ValidationSummary>(2);
                builder.CloseComponent();
            }));

    [Fact]
    public async Task ValidateAsync_InvalidModel_ShowsTheSharedValidatorMessages()
    {
        // Arrange
        Services.AddSharedValidators();
        Services.AddScoped<IValidationSettingsProvider>(_ => new FakeValidationSettingsProvider(AppSettingKeys.DoNotAllowDoctorWithoutSpecialty));
        var cut = RenderForm(new DoctorCreateRequest());

        // Act
        var isValid = await cut.InvokeAsync(() => _validator!.ValidateAsync());

        // Assert
        isValid.ShouldBeFalse();
        var messages = cut.FindAll("li.validation-message").Select(li => li.TextContent).ToList();
        messages.ShouldContain(string.Format(Resource.PropertyRequired, Resource.FirstName));
        messages.ShouldContain(string.Format(Resource.PropertyRequired, Resource.Specialty));
    }

    [Fact]
    public async Task ValidateAsync_ValidModel_ReturnsTrue()
    {
        // Arrange
        Services.AddSharedValidators();
        Services.AddScoped<IValidationSettingsProvider>(_ => new FakeValidationSettingsProvider());
        var cut = RenderForm(new DoctorCreateRequest
        {
            FirstName = "Gregory",
            LastName = "House",
            Email = "house@ppth.org",
            Password = "Vic0din!",
            ConfirmPassword = "Vic0din!"
        });

        // Act
        var isValid = await cut.InvokeAsync(() => _validator!.ValidateAsync());

        // Assert
        isValid.ShouldBeTrue();
        cut.FindAll("li.validation-message").ShouldBeEmpty();
    }
}
