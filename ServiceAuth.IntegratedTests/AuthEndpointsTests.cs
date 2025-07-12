using ServiceAuth.Application.Models;
using System;
using System.Net.Http.Json;

namespace ServiceAuth.IntegratedTests;

public class AuthEndpointsTests : IClassFixture<ServiceAuthApiFactory>
{
    private readonly ServiceAuthApiFactory _factory;
    private readonly HttpClient _client;
    private readonly Random _random;

    public AuthEndpointsTests(ServiceAuthApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _random = new Random();
    }

    [Fact]
    public async Task Signup_DeveCriarUsuarioERetornar201()
    {
        var signup = CreateUser();
        var response = await _client.PostAsJsonAsync("/auth/signup", signup);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Fact]
    public async Task Login_DeveRetornarTokenComCredenciaisValidas()
    {
        // Primeiro cria o usuário
        var signup = CreateUser();
        var signupResponse = await _client.PostAsJsonAsync("/auth/signup", signup);
        signupResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // Depois tenta logar
        var login = new {
            Email = signup.Email,
            Password = signup.Password
        };
        var response = await _client.PostAsJsonAsync("/auth/login", login);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<LoginResponseModel>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task ForgotPassword_DeveRetornar200ParaEmailExistente()
    {
        var signup = CreateUser();
        var signupResponse = await _client.PostAsJsonAsync("/auth/signup", signup);
        signupResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var forgot = new
        {
            Email = signup.Email
        };
        var response = await _client.PostAsJsonAsync("/auth/forgot-password", forgot);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task ResetPassword_DeveRetornar200ComDadosValidos()
    {
        var signup = CreateUser();
        var signupResponse = await _client.PostAsJsonAsync("/auth/signup", signup);
        signupResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);


        var reset = new ResetPasswordModel {
            Token = token,
            NewPassword = "NovaSenha@123",
            ConfirmPassword = "NovaSenha@123"
        };
        var response = await _client.PostAsJsonAsync("/auth/reset-password", reset);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    private SignupModel CreateUser()
    {
        var number = _random.Next(1, 20000);
        return new SignupModel
        {
            Name = $"Teste User {number}",
            Email = $"user{number}@email.com",
            Password = "Senha@123",
            ConfirmPassword = "Senha@123",
            CPF = "12345678909"
        };
    }

    private class LoginResponseModel
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
} 