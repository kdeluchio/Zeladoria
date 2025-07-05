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
    
    public int CustomerId { get; set; }
    public CustomerResponseModel? Customer { get; set; }
    
    public int ServiceId { get; set; }
    public ServiceResponseModel? Service { get; set; }
    
    public int TechnicianId { get; set; }
    public TechnicianResponseModel? Technician { get; set; }
    
    public string Feedback { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class CustomerResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public class ServiceResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TechnicianResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
} 