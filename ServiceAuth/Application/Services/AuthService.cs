using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using ServiceAuth.Application.Models;
using ServiceAuth.Domain.Entities;
using ServiceAuth.Domain.Enums;
using ServiceAuth.Domain.Interfaces;
using ServiceAuth.Domain.Models;

namespace ServiceAuth.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<LoginModel> _loginValidator;
    private readonly IValidator<SignupModel> _signupValidator;
    private readonly IValidator<ResetPasswordModel> _resetPasswordValidator;
    private readonly IConfiguration _configuration;
    private readonly IQueueProducerService _producer;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IUserRepository userRepository,
        IValidator<LoginModel> loginValidator,
        IValidator<SignupModel> signupValidator,
        IValidator<ResetPasswordModel> resetPasswordValidator,
        IConfiguration configuration,
        IQueueProducerService producer,
        JwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _loginValidator = loginValidator;
        _signupValidator = signupValidator;
        _resetPasswordValidator = resetPasswordValidator;
        _configuration = configuration;
        _producer = producer;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<LoginResponseModel>> LoginAsync(LoginModel loginModel)
    {
        var validation = await _loginValidator.ValidateAsync(loginModel);
        if (!validation.IsValid)
        {
            var validationErrors = validation.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            return Result<LoginResponseModel>.Failure(validationErrors, ErrorType.Validation);
        }

        var user = await _userRepository.GetByEmailAsync(loginModel.Email);
        if (user == null)
        {
            return Result<LoginResponseModel>.Unauthorized("Email ou senha inválidos");
        }

        if (!user.IsActive)
        {
            return Result<LoginResponseModel>.Unauthorized("Usuário inativo");
        }

        if (!VerifyPassword(loginModel.Password, user.PasswordHash))
        {
            return Result<LoginResponseModel>.Unauthorized("Email ou senha inválidos");
        }

        var userResponse = MapToUserResponse(user);
        var token = _jwtTokenGenerator.GenerateToken(userResponse);

        return Result<LoginResponseModel>.Success(new LoginResponseModel
        {
            Token = token,
            User = userResponse
        });
    }

    public async Task<Result<UserResponseModel>> SignupAsync(SignupModel signupModel)
    {
        var validation = await _signupValidator.ValidateAsync(signupModel);
        if (!validation.IsValid)
        {
            var validationErrors = validation.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            return Result<UserResponseModel>.Failure(validationErrors, ErrorType.Validation);
        }

        var existingUser = await _userRepository.GetByEmailAsync(signupModel.Email);
        if (existingUser != null)
        {
            return Result<UserResponseModel>.Conflict("Email já cadastrado");
        }

        var passwordHash = HashPassword(signupModel.Password);

        var user = new User
        {
            Name = signupModel.Name,
            Email = signupModel.Email,
            PasswordHash = passwordHash,
            Role = "User",
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            CPF = signupModel.CPF,
            Phone = signupModel.Phone,
        };

        var createdUser = await _userRepository.CreateAsync(user);
        return Result<UserResponseModel>.Success(MapToUserResponse(createdUser));
    }

    public async Task<Result<string>> ForgotPasswordAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result<string>.Failure("Email é obrigatório", ErrorType.Validation);
        }

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return Result<string>.Success("Se o email estiver cadastrado, você receberá um link para reset de senha");
        }

        var resetToken = GenerateResetToken();
        user.ResetPasswordToken = resetToken;
        user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(24);

        await _userRepository.UpdateAsync(user);

        //Event asincrono para enviar o email
        await _producer.SendAsync(new QueueMessage(user.Email, resetToken));

        return Result<string>.Success($"Email enviado com o token");
    }

    public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordModel resetPasswordModel)
    {
        var validation = await _resetPasswordValidator.ValidateAsync(resetPasswordModel);
        if (!validation.IsValid)
        {
            var validationErrors = validation.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            return Result<bool>.Failure(validationErrors, ErrorType.Validation);
        }

        var user = await _userRepository.GetByResetTokenAsync(resetPasswordModel.Token);
        if (user == null)
        {
            return Result<bool>.NotFound("Token de reset inválido");
        }

        if (user.ResetPasswordTokenExpiry < DateTime.UtcNow)
        {
            return Result<bool>.Failure("Token de reset expirado", ErrorType.Validation);
        }

        user.PasswordHash = HashPassword(resetPasswordModel.NewPassword);
        user.ResetPasswordToken = null;
        user.ResetPasswordTokenExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return Result<bool>.Success(true);
    }

    public async Task<Result<UserResponseModel>> GetUserByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null
            ? Result<UserResponseModel>.Success(MapToUserResponse(user))
            : Result<UserResponseModel>.NotFound($"Usuário com ID {id} não encontrado");
    }



    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        var passwordHash = HashPassword(password);
        return passwordHash == hash;
    }

    private static string GenerateResetToken()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 32)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static UserResponseModel MapToUserResponse(User user)
    {
        return new UserResponseModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            IsActive = user.IsActive,
            CPF = user.CPF,
            Phone = user.Phone
        };
    }
}