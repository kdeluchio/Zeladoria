using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Domain.Interfaces;

public interface IServiceRepository
{
    Task<Service> CreateAsync(Service service);
    Task<Service?> GetByIdAsync(string id);
    Task<IEnumerable<Service>> GetAllAsync();
    Task<Service> UpdateAsync(Service service);
    Task<bool> DeleteAsync(string id);
}