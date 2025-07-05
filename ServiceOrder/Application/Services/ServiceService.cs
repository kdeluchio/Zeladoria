using ServiceOrder.Application.Interfaces;
using ServiceOrder.Application.Models;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Application.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ServiceResponseModel> CreateServiceAsync(CreateServiceModel createServiceModel)
    {
        var service = new Service(createServiceModel.Name);
        var createdService = await _serviceRepository.CreateAsync(service);
        return MapToResponseModel(createdService);
    }

    public async Task<ServiceResponseModel?> GetServiceByIdAsync(string id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        return service != null ? MapToResponseModel(service) : null;
    }

    public async Task<IEnumerable<ServiceResponseModel>> GetAllServicesAsync()
    {
        var services = await _serviceRepository.GetAllAsync();
        return services.Select(MapToResponseModel);
    }

    public async Task<ServiceResponseModel> UpdateServiceAsync(string id, CreateServiceModel updateServiceModel)
    {
        var existingService = await _serviceRepository.GetByIdAsync(id);
        if (existingService == null)
            throw new ArgumentException("Service not found", nameof(id));

        existingService.UpdateName(updateServiceModel.Name);
        var updatedService = await _serviceRepository.UpdateAsync(existingService);
        return MapToResponseModel(updatedService);
    }

    public async Task<bool> DeleteServiceAsync(string id)
    {
        return await _serviceRepository.DeleteAsync(id);
    }

    private static ServiceResponseModel MapToResponseModel(Service service)
    {
        return new ServiceResponseModel
        {
            Id = service.Id,
            Name = service.Name
        };
    }
} 