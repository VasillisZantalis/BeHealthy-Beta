using System.Text.RegularExpressions;

namespace BeHealthy.Validation.Common;

/// <summary>
/// Reusable rule chains for the fields that appear on many requests (names, emails, passwords,
/// ids, ...). Each chain stops at the first failure, so a field shows one message at a time.
/// </summary>
public static partial class RuleBuilderExtensions
{
    // E.164: optional '+', then up to 15 digits, not starting with 0.
    [GeneratedRegex(@"^\+?[1-9]\d{1,14}$")]
    private static partial Regex PhoneNumberRegex();

    public static IRuleBuilderOptions<T, string> RequiredText<T>(this IRuleBuilderInitial<T, string> rule, string fieldName, int maxLength)
        => rule
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ValidationMessages.Required(fieldName))
            .MaximumLength(maxLength).WithMessage(ValidationMessages.MaxLength(fieldName, maxLength));

    public static IRuleBuilderOptions<T, string?> OptionalText<T>(this IRuleBuilder<T, string?> rule, string fieldName, int maxLength)
        => rule
            .MaximumLength(maxLength).WithMessage(ValidationMessages.MaxLength(fieldName, maxLength));

    public static IRuleBuilderOptions<T, string> PersonName<T>(this IRuleBuilderInitial<T, string> rule, string fieldName)
        => rule.RequiredText(fieldName, FieldLengths.PersonName);

    public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilderInitial<T, string> rule)
        => rule
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ValidationMessages.Required(Resource.Email))
            .MaximumLength(FieldLengths.Email).WithMessage(ValidationMessages.MaxLength(Resource.Email, FieldLengths.Email))
            .EmailAddress().WithMessage(Resource.InvalidEmailFormat);

    /// <summary>Mirrors <see cref="PasswordPolicy"/>, which is also what Identity is configured with.</summary>
    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilderInitial<T, string> rule)
    {
        var options = rule
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ValidationMessages.Required(Resource.Password))
            .MinimumLength(PasswordPolicy.RequiredLength).WithMessage(string.Format(Resource.PasswordTooShort, PasswordPolicy.RequiredLength));

        // The same character classes Identity's PasswordValidator checks (char.IsUpper, IsLower, IsDigit, !IsLetterOrDigit).
        if (PasswordPolicy.RequireUppercase)
        {
            options = options.Matches(@"\p{Lu}").WithMessage(Resource.PasswordNeedsUppercase);
        }

        if (PasswordPolicy.RequireLowercase)
        {
            options = options.Matches(@"\p{Ll}").WithMessage(Resource.PasswordNeedsLowercase);
        }

        if (PasswordPolicy.RequireDigit)
        {
            options = options.Matches(@"\d").WithMessage(Resource.PasswordNeedsDigit);
        }

        if (PasswordPolicy.RequireNonAlphanumeric)
        {
            options = options.Matches(@"[^\p{L}\p{Nd}]").WithMessage(Resource.PasswordNeedsNonAlphanumericCharacter);
        }

        return options;
    }

    /// <summary>Optional: an empty value is allowed, anything else must be a valid phone number.</summary>
    public static IRuleBuilderOptions<T, string?> PhoneNumber<T>(this IRuleBuilder<T, string?> rule)
        => rule
            .Must(phone => string.IsNullOrEmpty(phone) || PhoneNumberRegex().IsMatch(phone))
            .WithMessage(ValidationMessages.InvalidFormat(Resource.PhoneNumber));

    public static IRuleBuilderOptions<T, string?> Image<T>(this IRuleBuilder<T, string?> rule)
        => rule
            .MaximumLength(FieldLengths.Image).WithMessage(Resource.ImageTooLarge);

    /// <summary>A required foreign key: 0 is what an unselected dropdown sends.</summary>
    public static IRuleBuilderOptions<T, int> RequiredReference<T>(this IRuleBuilder<T, int> rule, string entityName)
        => rule
            .GreaterThan(0).WithMessage(ValidationMessages.Required(entityName));

    /// <summary>An optional foreign key: <c>null</c> means "none", anything else must be a real id.</summary>
    public static IRuleBuilderOptions<T, int?> OptionalReference<T>(this IRuleBuilder<T, int?> rule, string entityName)
        => rule
            .Must(id => id is null or > 0).WithMessage(ValidationMessages.NotFound(entityName));

    /// <summary>The id of the entity being updated.</summary>
    public static IRuleBuilderOptions<T, int> EntityId<T>(this IRuleBuilder<T, int> rule)
        => rule
            .GreaterThan(0).WithMessage(Resource.InvalidData);

    public static IRuleBuilderOptions<T, TEnum> DefinedEnum<T, TEnum>(this IRuleBuilder<T, TEnum> rule, string fieldName)
        where TEnum : struct, Enum
        => rule
            .IsInEnum().WithMessage(ValidationMessages.InvalidValue(fieldName));

    /// <summary>
    /// Allows a day of slack so a date picked in the user's local time zone is never rejected
    /// because the server compares it with its own clock.
    /// </summary>
    public static IRuleBuilderOptions<T, DateTime> NotInTheFuture<T>(this IRuleBuilder<T, DateTime> rule, string fieldName)
        => rule
            .Must(date => date <= DateTime.UtcNow.AddDays(1)).WithMessage(ValidationMessages.NotInTheFuture(fieldName));
}
