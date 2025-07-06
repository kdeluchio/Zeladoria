using ServiceAuth.Domain.Entities;

namespace ServiceAuth.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(User user);
    Task<bool> DeleteAsync(string id);
    Task<User?> GetByResetTokenAsync(string resetToken);
} 