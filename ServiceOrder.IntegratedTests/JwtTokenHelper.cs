using ServiceAuth.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ServiceOrder.IntegratedTests;

public static class JwtTokenHelper
{
    private const string SecretKey = "fiap-sw-eng-dev-trabalho-zeladoria-urb-secret-key-for-jwt-signing";
    private const string Issuer = "ServiceAuth";
    private const string Audience = "ServiceAuth";
    private const int ExpirationHours = 24;

    public static string GenerateTestToken()
    {
        return GenerateToken(
            userId: "507f1f77bcf86cd799439011",
            userName: "Usuário Teste",
            userEmail: "teste@exemplo.com",
            userRole: "User"
        );
    }

    public static string GenerateTestToken(string userId, string userName, string userEmail, string userRole)
    {
        return GenerateToken(userId, userName, userEmail, userRole);
    }

    private static string GenerateToken(string userId, string userName, string userEmail, string userRole)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, userEmail),
            new Claim(ClaimTypes.Role, userRole)
        };

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(ExpirationHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
} 