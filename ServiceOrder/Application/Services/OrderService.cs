using FluentValidation;
using MongoDB.Bson;
using ServiceOrder.Application.Models;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Enums;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Domain.Models;

namespace ServiceOrder.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IValidator<CreateOrderModel> _validator;

    public OrderService(IOrderRepository orderRepository, 
                        IValidator<CreateOrderModel> validator,
                        IServiceRepository serviceRepository)
    {
        _orderRepository = orderRepository;
        _validator = validator;
        _serviceRepository = serviceRepository;
    }

    public async Task<Result<OrderResponseModel>> CreateOrderAsync(CreateOrderModel request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var validationErrors = validation.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            return Result<OrderResponseModel>.Failure(validationErrors, ErrorType.Validation);
        }

        var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
        if (service == null)
        {
            return Result<OrderResponseModel>.Failure("Servico nao cadastrado", ErrorType.Validation);
        }

        var order = new Order(
            request.CustomerId,
            service,
            request.Description,
            request.Address,
            request.NumberAddress,
            request.Latitude,
            request.Longitude
        );

        var createdOrder = await _orderRepository.CreateAsync(order);
        return Result<OrderResponseModel>.Success(MapToResponseModel(createdOrder));
    }

    public async Task<Result<OrderResponseModel>> GetOrderByIdAsync(string id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order != null
            ? Result<OrderResponseModel>.Success(MapToResponseModel(order))
            : Result<OrderResponseModel>.NotFound($"Pedido com ID {id} não encontrado");
    }

    public async Task<Result<IEnumerable<OrderResponseModel>>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        var responseModels = orders.Select(MapToResponseModel);
        return Result<IEnumerable<OrderResponseModel>>.Success(responseModels);
    }

    public async Task<Result<OrderResponseModel>> UpdateOrderAsync(string id, CreateOrderModel request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var validationErrors = validation.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            return Result<OrderResponseModel>.Failure(validationErrors, ErrorType.Validation);
        }

        var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
        if (service == null)
        {
            return Result<OrderResponseModel>.Failure("Servico nao cadastrado", ErrorType.Validation);
        }

        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            return Result<OrderResponseModel>.NotFound($"Pedido com ID {id} não encontrado");

        var updateResult = order.TryUpdate(request.Description, request.Address, request.NumberAddress, request.Latitude, request.Longitude);
        if (!updateResult.IsSuccess)
            return Result<OrderResponseModel>.Failure(updateResult.Errors.First(), ErrorType.Validation);

        var orderToUpdate = await _orderRepository.UpdateAsync(order);
        return Result<OrderResponseModel>.Success(MapToResponseModel(orderToUpdate));
    }

    public async Task<Result<bool>> DeleteOrderAsync(string id)
    {
        var deleted = await _orderRepository.DeleteAsync(id);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.NotFound($"Pedido com ID {id} não encontrado");
    }

    private static OrderResponseModel MapToResponseModel(Order order)
    {
        return new OrderResponseModel
        {
            Id = order.Id,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            Description = order.Description,
            Address = order.Address,
            NumberAddress = order.NumberAddress,
            Latitude = order.Latitude,
            Longitude = order.Longitude,
            CustomerId = order.CustomerId,
            Service = new ServiceResponseModel
            {
                Id = order.Service.Id,
                Name = order.Service.Name
            },
            TechnicianId = order.TechnicianId,
            Feedback = order.Feedback,
            UpdatedAt = order.UpdatedAt,
            CompletedAt = order.CompletedAt
        };
    }
}