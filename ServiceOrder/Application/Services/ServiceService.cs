using FluentValidation;
using MongoDB.Bson;
using ServiceOrder.Application.Models;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Enums;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Domain.Models;

namespace ServiceOrder.Application.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IValidator<CreateServiceModel> _validator;

    public ServiceService(IServiceRepository serviceRepository, IValidator<CreateServiceModel> validator)
    {
        _serviceRepository = serviceRepository;
        _validator = validator;
    }

    public async Task<Result<ServiceResponseModel>> CreateServiceAsync(CreateServiceModel request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var validationErrors = validation.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            return Result<ServiceResponseModel>.Failure(validationErrors, ErrorType.Validation);
        }

        var service = new Service(request.Name);
        var createdService = await _serviceRepository.CreateAsync(service);
        return Result<ServiceResponseModel>.Success(MapToResponseModel(createdService));
    }

    public async Task<Result<ServiceResponseModel>> GetServiceByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return Result<ServiceResponseModel>.NotFound($"Serviço com ID {id} não encontrado");

        var service = await _serviceRepository.GetByIdAsync(id);
        return service != null 
            ? Result<ServiceResponseModel>.Success(MapToResponseModel(service))
            : Result<ServiceResponseModel>.NotFound($"Serviço com ID {id} não encontrado");
    }

    public async Task<Result<IEnumerable<ServiceResponseModel>>> GetAllServicesAsync()
    {
        var services = await _serviceRepository.GetAllAsync();
        var responseModels = services.Select(MapToResponseModel);
        return Result<IEnumerable<ServiceResponseModel>>.Success(responseModels);
    }

    public async Task<Result<ServiceResponseModel>> UpdateServiceAsync(string id, CreateServiceModel request)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return Result<ServiceResponseModel>.NotFound($"Serviço com ID {id} não encontrado");

        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var validationErrors = validation.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            return Result<ServiceResponseModel>.Failure(validationErrors, ErrorType.Validation);
        }

        var existingService = await _serviceRepository.GetByIdAsync(id);
        if (existingService == null)
            return Result<ServiceResponseModel>.NotFound($"Serviço com ID {id} não encontrado");

        var updateResult = existingService.TryUpdateName(request.Name);
        if (!updateResult.IsSuccess)
            return Result<ServiceResponseModel>.Failure(updateResult.Errors.First(), ErrorType.Validation);

        var updatedService = await _serviceRepository.UpdateAsync(existingService);
        return Result<ServiceResponseModel>.Success(MapToResponseModel(updatedService));
    }

    public async Task<Result<bool>> DeleteServiceAsync(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return Result<bool>.NotFound($"Serviço com ID {id} não encontrado");

        var deleted = await _serviceRepository.DeleteAsync(id);
        return deleted 
            ? Result<bool>.Success(true)
            : Result<bool>.NotFound($"Serviço com ID {id} não encontrado");
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