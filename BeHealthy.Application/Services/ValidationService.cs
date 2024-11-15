using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Services.Interfaces;
using FluentValidation;

namespace BeHealthy.Application.Services;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ServiceResponse> ValidateAsync<T>(T dto)
    {
        //var validator = _serviceProvider.GetService<IValidator<T>>();

        //if (validator == null)
        //{
        //    return ServiceResponse.Failed();
        //}

        //var validationResult = await validator.ValidateAsync(dto);

        //if (!validationResult.IsValid)
        //{
        //    var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
        //    return ServiceResponse.Failed(errorMessage);
        //}

        return ServiceResponse.Successful();
    }
}
