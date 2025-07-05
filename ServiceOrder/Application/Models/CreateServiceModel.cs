using System.ComponentModel.DataAnnotations;

namespace ServiceOrder.Application.Models;

public class CreateServiceModel
{
    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string Name { get; set; } = string.Empty;
} 