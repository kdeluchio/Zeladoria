using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceOrder.Domain.Models;

namespace ServiceOrder.Domain.Entities;

public class Service
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; }
    
    [BsonRequired]
    public string Name { get; private set; }
    

    public Service(string name)
    {
        Id = ObjectId.GenerateNewId().ToString();
        Name = name;
    }

    public Result TryUpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("Nome do serviço não pode estar vazio");
        
        Name = name;
        return Result.Success();
    }
}
