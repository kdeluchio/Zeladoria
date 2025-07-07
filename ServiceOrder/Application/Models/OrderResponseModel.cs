using ServiceOrder.Domain.Enums;

namespace ServiceOrder.Application.Models;

public class OrderResponseModel
{
    public string Id { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string NumberAddress { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public CustomerResponseModel? Customer { get; set; }
    
    public ServiceResponseModel? Service { get; set; }
    
    public TechnicianResponseModel? Technician { get; set; }
    
    public string Feedback { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
