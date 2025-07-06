using ServiceAuth.Application.Models;
using ServiceAuth.Domain.Models;

namespace ServiceAuth.Domain.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponseModel>> LoginAsync(LoginModel loginModel);
    Task<Result<UserResponseModel>> SignupAsync(SignupModel signupModel);
    Task<Result<string>> ForgotPasswordAsync(string email);
    Task<Result<bool>> ResetPasswordAsync(ResetPasswordModel resetPasswordModel);
    Task<Result<UserResponseModel>> GetUserByIdAsync(string id);
    Task<Result<string>> GenerateJwtTokenAsync(UserResponseModel user);
} 