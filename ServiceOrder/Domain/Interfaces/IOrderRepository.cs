using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
    Task<Order?> GetByIdAsync(string id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order> UpdateAsync(Order order);
    Task<bool> DeleteAsync(string id);
}