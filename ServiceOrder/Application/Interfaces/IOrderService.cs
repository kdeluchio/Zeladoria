using ServiceOrder.Application.Models;

namespace ServiceOrder.Application.Interfaces;

public interface IOrderService
{
    Task<OrderResponseModel> CreateOrderAsync(CreateOrderModel createOrderModel);
    Task<OrderResponseModel?> GetOrderByIdAsync(string id);
    Task<IEnumerable<OrderResponseModel>> GetAllOrdersAsync();
    Task<OrderResponseModel> UpdateOrderAsync(string id, CreateOrderModel updateOrderModel);
    Task<bool> DeleteOrderAsync(string id);
} 