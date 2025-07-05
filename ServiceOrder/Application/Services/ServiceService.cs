using FluentValidation;
using ServiceOrder.Application.Interfaces;
using ServiceOrder.Application.Models;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Application.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IValidator<CreateServiceModel> _validator;
    private bool _hasError;
    private Dictionary<string, string[]> _errors;

    public bool HasError { get => _hasError; }
    public Dictionary<string, string[]> Errors { get => _errors; }

    public ServiceService(IServiceRepository serviceRepository, IValidator<CreateServiceModel> validator)
    {
        _serviceRepository = serviceRepository;
        _validator = validator;
        _errors = new Dictionary<string, string[]>();
    }

    public async Task<ServiceResponseModel> CreateServiceAsync(CreateServiceModel createServiceModel)
    {
        var validationResult = await _validator.ValidateAsync(createServiceModel);
        if (!validationResult.IsValid)
        {
            _hasError = true;
            _errors = validationResult.Errors
                .ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage });
            return default;
        }

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
        var validationResult = await _validator.ValidateAsync(updateServiceModel);
        if (!validationResult.IsValid)
        {
            _hasError = true;
            _errors = validationResult.Errors
                .ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage });
            return default;
        }
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