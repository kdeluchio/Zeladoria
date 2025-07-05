using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Service name cannot be empty", nameof(name));
        
        Name = name;
    }

}
