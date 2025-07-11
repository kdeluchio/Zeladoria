using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceOrder.Domain.Enums;
using ServiceOrder.Domain.Models;

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
    public Customer Customer { get; private set; }
    public Service Service { get; private set; }
    public Technician Technician { get; private set; }
    public string Feedback { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public Order(Customer customer, Service service, string description, string address, string numberAddress, double latitude, double longitude)
    {
        Id = ObjectId.GenerateNewId().ToString();
        Service = service;
        Customer = customer;
        Description = description;
        Address = address;
        NumberAddress = numberAddress;
        Latitude = latitude;
        Longitude = longitude;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetInProgress(Technician technician)
    {
        Technician = technician;
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

    public Result TryUpdate(string description, string address, string numberAddress, double latitude, double longitude)
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure("Só é possível editar pedidos com status Pendente");

        Description = description;
        Address = address;
        NumberAddress = numberAddress;
        Latitude = latitude;
        Longitude = longitude;
        
        return Result.Success();
    }
}
