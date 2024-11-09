using BeHealthy.Persistance;

namespace BeHealthy.Services.Interfaces
{
    public interface IValidationService
    {
        Task<ServiceResponse> ValidateAsync<T>(T dto);
    }
}
