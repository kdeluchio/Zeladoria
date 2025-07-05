using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceOrder.Domain.Enums;

namespace ServiceOrder.Domain.Entities;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } 
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Description { get; private set; }
    public string Address { get; private set; }
    public string NumberAddress { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    
    public int CustomerId { get; private set; }
    public virtual Customer Customer { get; set; }
    
    public int ServiceId { get; private set; }
    public virtual Service Service { get; set; }
    
    public int TechnicianId { get; private set; }
    public virtual Technician? Technician { get; set; }
    
    public string Feedback { get; private set; }
   
    public DateTime? UpdatedAt { get; private set; }
    
    public DateTime? CompletedAt { get; private set; }

    public Order(int customerId, string description, string address, string numberAddress, double latitude, double longitude)
    {
        Id = ObjectId.GenerateNewId().ToString();
        CustomerId = customerId;
        Description = description;
        Address = address;
        NumberAddress = numberAddress;
        Latitude = latitude;
        Longitude = longitude;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetInProgress(int technicianId)
    {
        TechnicianId = technicianId;
        Status = OrderStatus.InProgress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCompleted(string feedback)
    {
        Status = OrderStatus.Completed;
        Feedback = feedback;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCancelled()
    {
        Status = OrderStatus.Cancelled;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
