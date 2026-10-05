using BeHealthy.Shared.Locales;
using BeHealthy.Validation.Common;
using FluentValidation;

namespace BeHealthy.Application.Validators;

/// <summary>
/// Rules that need the database. They skip values the shared validator already rejects
/// (an id of 0 or less, an empty email) so the user gets one message per field, not two.
/// </summary>
public static class ServerRuleExtensions
{
    public static IRuleBuilderOptions<T, int> MustExist<T, TEntity>(this IRuleBuilder<T, int> rule, IGenericRepository<TEntity> repository, string entityName)
        where TEntity : class
        => rule
            .MustAsync(async (id, cancellationToken) => id <= 0 || await repository.ExistsAsync(id, cancellationToken))
            .WithMessage(ValidationMessages.NotFound(entityName));

    public static IRuleBuilderOptions<T, int?> MustExist<T, TEntity>(this IRuleBuilder<T, int?> rule, IGenericRepository<TEntity> repository, string entityName)
        where TEntity : class
        => rule
            .MustAsync(async (id, cancellationToken) => id is not > 0 || await repository.ExistsAsync(id.Value, cancellationToken))
            .WithMessage(ValidationMessages.NotFound(entityName));

    public static IRuleBuilderOptions<T, string> MustBeUnusedEmail<T>(this IRuleBuilder<T, string> rule, IUserService userService)
        => rule
            .MustAsync(async (email, cancellationToken) => string.IsNullOrWhiteSpace(email) || !await userService.IsEmailInUseAsync(email, cancellationToken))
            .WithMessage(Resource.EmailAlreadyUsed);
}
