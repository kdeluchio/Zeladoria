using MongoDB.Bson;
using MongoDB.Driver;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Interfaces;

namespace ServiceOrder.Infra.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _orders;

    public OrderRepository(IMongoDatabase database)
    {
        _orders = database.GetCollection<Order>("orders");
    }

    public async Task<Order> CreateAsync(Order order)
    {
        await _orders.InsertOneAsync(order);
        return order;
    }

    public async Task<Order?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return default;

        return await _orders.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _orders.Find(_ => true).ToListAsync();
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.Id, order.Id);
        await _orders.ReplaceOneAsync(filter, order);
        return order;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return default;

        var result = await _orders.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
} 