namespace ServiceAuth.Application.Models;

public class LoginResponseModel
{
    public string Token { get; set; } = string.Empty;
    public UserResponseModel User { get; set; } = new();
} 