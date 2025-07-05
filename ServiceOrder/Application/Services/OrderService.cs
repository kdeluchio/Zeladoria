using ServiceOrder.Application.Interfaces;
using ServiceOrder.Application.Models;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponseModel> CreateOrderAsync(CreateOrderModel createOrderModel)
    {
        var order = new Order(
            createOrderModel.CustomerId,
            createOrderModel.Description,
            createOrderModel.Address,
            createOrderModel.NumberAddress,
            createOrderModel.Latitude,
            createOrderModel.Longitude
        );

        var createdOrder = await _orderRepository.CreateAsync(order);

        return MapToResponseModel(createdOrder);
    }

    public async Task<OrderResponseModel?> GetOrderByIdAsync(string id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order != null ? MapToResponseModel(order) : null;
    }

    public async Task<IEnumerable<OrderResponseModel>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(MapToResponseModel);
    }

    public async Task<OrderResponseModel> UpdateOrderAsync(string id, CreateOrderModel updateOrderModel)
    {
        var existingOrder = await _orderRepository.GetByIdAsync(id);
        if (existingOrder == null)
            throw new ArgumentException("Order not found", nameof(id));

        var updatedOrder = new Order(
            updateOrderModel.CustomerId,
            updateOrderModel.Description,
            updateOrderModel.Address,
            updateOrderModel.NumberAddress,
            updateOrderModel.Latitude,
            updateOrderModel.Longitude
        );

        var orderToUpdate = await _orderRepository.UpdateAsync(updatedOrder);
        return MapToResponseModel(orderToUpdate);
    }

    public async Task<bool> DeleteOrderAsync(string id)
    {
        return await _orderRepository.DeleteAsync(id);
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
            Customer = order.Customer != null ? new CustomerResponseModel
            {
                Id = order.Customer.Id,
                Name = order.Customer.Name,
                CPF = order.Customer.CPF,
                Email = order.Customer.Email,
                Phone = order.Customer.Phone
            } : null,
            ServiceId = order.ServiceId,
            Service = order.Service != null ? new ServiceResponseModel
            {
                Id = order.Service.Id,
                Name = order.Service.Name
            } : null,
            TechnicianId = order.TechnicianId,
            Technician = order.Technician != null ? new TechnicianResponseModel
            {
                Id = order.Technician.Id,
                Name = order.Technician.Name,
                CompanyCode = order.Technician.CompanyCode,
                Email = order.Technician.Email,
                Phone = order.Technician.Phone
            } : null,
            Feedback = order.Feedback,
            UpdatedAt = order.UpdatedAt,
            CompletedAt = order.CompletedAt
        };
    }
} 