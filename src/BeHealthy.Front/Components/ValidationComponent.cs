using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Front.Components;

public class ValidationComponent : ComponentBase
{
    private ValidationMessageStore? messageStore;

    [CascadingParameter]
    private EditContext? CurrentEditContext { get; set; }

    protected override void OnInitialized()
    {
        if (CurrentEditContext is null)
        {
            throw new InvalidOperationException(
                $"{nameof(ValidationComponent)} requires a cascading " +
                $"parameter of type {nameof(EditContext)}. " +
                $"For example, you can use {nameof(ValidationComponent)} " +
                $"inside an {nameof(EditForm)}.");
        }

        messageStore = new(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += (s, e) =>
            messageStore?.Clear();
        CurrentEditContext.OnFieldChanged += (s, e) =>
            messageStore?.Clear(e.FieldIdentifier);
    }

    public void DisplayErrors(Dictionary<string, List<string>> errors)
    {
        if (CurrentEditContext is not null)
        {
            foreach (var err in errors)
            {
                messageStore?.Add(CurrentEditContext.Field(err.Key), err.Value);
            }

            CurrentEditContext.NotifyValidationStateChanged();
        }
    }

    /// <summary>
    /// Shows the per-field errors the API returned (<see cref="Shared.Dtos.Common.ServiceResponse.ValidationErrors"/>).
    /// The keys are request property names, so they land on the inputs bound to properties of the same name.
    /// </summary>
    public void DisplayErrors(IReadOnlyDictionary<string, string[]>? errors)
    {
        if (errors is null)
        {
            return;
        }

        DisplayErrors(errors.ToDictionary(e => e.Key, e => e.Value.ToList()));
    }

    /// <summary>For an input bound to a nested property whose name differs from the request property.</summary>
    public void DisplayErrors(FieldIdentifier field, IEnumerable<string> messages)
    {
        if (CurrentEditContext is not null)
        {
            messageStore?.Add(field, messages);
            CurrentEditContext.NotifyValidationStateChanged();
        }
    }

    public void ClearErrors()
    {
        messageStore?.Clear();
        CurrentEditContext?.NotifyValidationStateChanged();
    }
}
