using MongoDB.Driver;
using ServiceAuth.Domain.Entities;
using ServiceAuth.Domain.Interfaces;

namespace ServiceAuth.Infra.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("Users");
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        return await _users.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        return await _users.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _users.Find(_ => true).ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<User?> UpdateAsync(User user)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
        var result = await _users.ReplaceOneAsync(filter, user);
        return result.IsAcknowledged && result.ModifiedCount > 0 ? user : null;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        var result = await _users.DeleteOneAsync(filter);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<User?> GetByResetTokenAsync(string resetToken)
    {
        var filter = Builders<User>.Filter.Eq(u => u.ResetPasswordToken, resetToken);
        return await _users.Find(filter).FirstOrDefaultAsync();
    }
} 