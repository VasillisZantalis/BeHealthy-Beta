using BeHealthy.Application.Dtos.Common;

namespace BeHealthy.Application.Services.Interfaces
{
    public interface IValidationService
    {
        Task<ServiceResponse> ValidateAsync<T>(T dto);
    }
}
