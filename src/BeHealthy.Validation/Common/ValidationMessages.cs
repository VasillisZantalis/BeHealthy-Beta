namespace BeHealthy.Validation.Common;

/// <summary>Localized validation messages, so the same rule is worded the same way everywhere.</summary>
public static class ValidationMessages
{
    public static string Required(string fieldName) => string.Format(Resource.PropertyRequired, fieldName);

    public static string MaxLength(string fieldName, int maxLength) => string.Format(Resource.PropertyMaxCharacters, fieldName, maxLength);

    public static string NotFound(string entityName) => string.Format(Resource.NotFoundEntity, entityName);

    public static string InvalidValue(string fieldName) => string.Format(Resource.PropertyInvalidValue, fieldName);

    public static string InvalidFormat(string fieldName) => string.Format(Resource.PropertyInvalidFormat, fieldName);

    public static string NotInTheFuture(string fieldName) => string.Format(Resource.PropertyCannotBeInTheFuture, fieldName);

    public static string Between(string fieldName, int min, int max) => string.Format(Resource.MustBeBetween, fieldName, min, max);
}
