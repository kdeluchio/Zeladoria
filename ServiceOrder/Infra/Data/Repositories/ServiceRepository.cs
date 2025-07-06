using MongoDB.Bson;
using MongoDB.Driver;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Interfaces;

namespace ServiceOrder.Infra.Data.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly IMongoCollection<Service> _services;

    public ServiceRepository(IMongoDatabase database)
    {
        _services = database.GetCollection<Service>("services");
    }

    public async Task<Service> CreateAsync(Service service)
    {
        await _services.InsertOneAsync(service);
        return service;
    }

    public async Task<Service?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return default;

        return await _services.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Service>> GetAllAsync()
    {
        return await _services.Find(_ => true).ToListAsync();
    }

    public async Task<Service> UpdateAsync(Service service)
    {
        var filter = Builders<Service>.Filter.Eq(x => x.Id, service.Id);
        await _services.ReplaceOneAsync(filter, service);
        return service;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return default;

        var result = await _services.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}