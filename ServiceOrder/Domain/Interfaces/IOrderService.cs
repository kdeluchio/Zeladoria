using ServiceOrder.Application.Models;
using ServiceOrder.Domain.Models;

namespace ServiceOrder.Domain.Interfaces;

public interface IOrderService
{
    Task<Result<OrderResponseModel>> CreateOrderAsync(CreateOrderModel request);
    Task<Result<OrderResponseModel>> GetOrderByIdAsync(string id);
    Task<Result<IEnumerable<OrderResponseModel>>> GetAllOrdersAsync();
    Task<Result<OrderResponseModel>> UpdateOrderAsync(string id, CreateOrderModel request);
    Task<Result<bool>> DeleteOrderAsync(string id);
}