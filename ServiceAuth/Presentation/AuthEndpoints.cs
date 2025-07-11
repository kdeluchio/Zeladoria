using ServiceAuth.Application.Models;
using ServiceAuth.Domain.Interfaces;
using ServiceAuth.Infra.Extensions;

namespace ServiceAuth.Presentation;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/auth/login", async (LoginModel loginModel, IAuthService authService) =>
        {
            var result = await authService.LoginAsync(loginModel);
            return result.ToHttpResult();
        });

        routes.MapPost("/auth/signup", async (SignupModel signupModel, IAuthService authService) =>
        {
            var result = await authService.SignupAsync(signupModel);
            return result.ToCreatedResult($"/auth/user/{result.Value?.Id}");
        });

        routes.MapPost("/auth/forgot-password", async (string email, IAuthService authService) =>
        {
            var result = await authService.ForgotPasswordAsync(email);
            return result.ToHttpResult();
        });

        routes.MapPost("/auth/reset-password", async (ResetPasswordModel resetPasswordModel, IAuthService authService) =>
        {
            var result = await authService.ResetPasswordAsync(resetPasswordModel);
            return result.ToHttpResult();
        });

    }
} 