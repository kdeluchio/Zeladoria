namespace ServiceOrder.Application.Models;

public class CreateOrderModel
{
    public int CustomerId { get; set; }
    public int ServiceId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string NumberAddress { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

