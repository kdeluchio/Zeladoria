using ServiceOrder.Application.Models;
using ServiceOrder.Domain.Models;

namespace ServiceOrder.Domain.Interfaces;

public interface IServiceService
{
    Task<Result<ServiceResponseModel>> CreateServiceAsync(CreateServiceModel request);
    Task<Result<ServiceResponseModel>> GetServiceByIdAsync(string id);
    Task<Result<IEnumerable<ServiceResponseModel>>> GetAllServicesAsync();
    Task<Result<ServiceResponseModel>> UpdateServiceAsync(string id, CreateServiceModel request);
    Task<Result<bool>> DeleteServiceAsync(string id);
}