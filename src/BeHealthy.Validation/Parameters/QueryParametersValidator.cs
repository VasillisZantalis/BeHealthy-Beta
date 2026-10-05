using BeHealthy.Shared.Parameters;

namespace BeHealthy.Validation.Parameters;

/// <summary>
/// Paging and search rules shared by every list endpoint. Without them <c>PageNumber = 0</c>
/// produces a negative Skip (500), <c>PageSize = 0</c> divides by zero and a huge page size
/// dumps the whole table.
/// </summary>
public abstract class QueryParametersValidatorBase<T> : AbstractValidator<T> where T : QueryParameters
{
    public const int MaxPageSize = 100;

    protected QueryParametersValidatorBase()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage(ValidationMessages.InvalidValue(Resource.PageNumber));

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, MaxPageSize).WithMessage(ValidationMessages.Between(Resource.PageSize, 1, MaxPageSize));

        RuleFor(x => x.SearchTerm).OptionalText(Resource.Search, FieldLengths.SearchTerm);
        RuleFor(x => x.OrderBy).OptionalText(Resource.SortBy, FieldLengths.OrderBy);
    }
}

// One concrete validator per parameter type: the validation filter looks validators up by the
// exact runtime type of the action argument.
public class QueryParametersValidator : QueryParametersValidatorBase<QueryParameters>;

public class DoctorQueryParametersValidator : QueryParametersValidatorBase<DoctorQueryParameters>;

public class PatientQueryParametersValidator : QueryParametersValidatorBase<PatientQueryParameters>;

public class AppointmentQueryParametersValidator : QueryParametersValidatorBase<AppointmentQueryParameters>;
