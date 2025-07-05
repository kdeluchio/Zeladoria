using System.ComponentModel.DataAnnotations;

namespace ServiceOrder.Application.Models;

public class CreateOrderModel
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "CustomerId deve ser maior que zero")]
    public int CustomerId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ServiceId deve ser maior que zero")]
    public int ServiceId { get; set; }
    
    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string Address { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    public string NumberAddress { get; set; } = string.Empty;
    
    [Required]
    [Range(-90, 90, ErrorMessage = "Latitude deve estar entre -90 e 90")]
    public double Latitude { get; set; }
    
    [Required]
    [Range(-180, 180, ErrorMessage = "Longitude deve estar entre -180 e 180")]
    public double Longitude { get; set; }
}

