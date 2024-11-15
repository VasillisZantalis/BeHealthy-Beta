using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Application.Helpers;

public class ValidationHelpers
{
    public List<ValidationResult> validationResults = new List<ValidationResult>();
    ValidationContext validationContext;
    object instance;

    public ValidationHelpers(object instance)
    {
        this.instance = instance;
    }

    public bool Validate()
    {
        return Validator.TryValidateObject(instance, validationContext, validationResults, true);
    }
}

/*
 * Use it by passing the object like
 * ValidationHelper helper = new ({ObjectToValidate})
 * bool isValid = helper.Validate()
 * string[] errors = {};
 * errors = helper.validationResults.Select(x => x.ErrorMessage).ToArray()
 * 
 */
