using ServiceOrder.Application.Models;

namespace ServiceOrder.Application.Interfaces;

public interface IServiceService
{
    Task<ServiceResponseModel> CreateServiceAsync(CreateServiceModel createServiceModel);
    Task<ServiceResponseModel?> GetServiceByIdAsync(string id);
    Task<IEnumerable<ServiceResponseModel>> GetAllServicesAsync();
    Task<ServiceResponseModel> UpdateServiceAsync(string id, CreateServiceModel updateServiceModel);
    Task<bool> DeleteServiceAsync(string id);
} 