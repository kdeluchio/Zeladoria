using ServiceOrder.Application.Models;
using System.Net.Http.Json;

namespace ServiceOrder.IntegratedTests;

public class ServiceEndpointsTests : IClassFixture<ServiceOrderApiFactory>
{
    private readonly ServiceOrderApiFactory _factory;
    private readonly HttpClient _client;
    private readonly Random _random;

    public ServiceEndpointsTests(ServiceOrderApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _random = new Random();
    }

    [Fact]
    public async Task GetAllServices_SemToken_DeveRetornar401()
    {
        // Remove qualquer header de autorização que possa estar definido
        _client.DefaultRequestHeaders.Authorization = null;
        
        var response = await _client.GetAsync("/service");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAllServices_DeveRetornar200()
    {
        var token = JwtTokenHelper.GenerateTestToken();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await _client.GetAsync("/service");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetServiceById_DeveRetornar200ParaIdValido()
    {
        var token = JwtTokenHelper.GenerateTestToken();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        // Primeiro cria um serviço
        var service = CreateService();
        var serviceResponse = await _client.PostAsJsonAsync("/service", service);
        serviceResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var serviceResult = await serviceResponse.Content.ReadFromJsonAsync<ServiceResponseModel>();
        serviceResult.Should().NotBeNull();

        // Busca o serviço criado
        var getResponse = await _client.GetAsync($"/service/{serviceResult!.Id}");
        getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetServiceById_DeveRetornar404ParaIdInvalido()
    {
        var token = JwtTokenHelper.GenerateTestToken();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await _client.GetAsync("/service/invalid-id");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateService_DeveCriarServicoERetornar201()
    {
        var token = JwtTokenHelper.GenerateTestToken();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var service = CreateService();
        var response = await _client.PostAsJsonAsync("/service", service);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<ServiceResponseModel>();
        result.Should().NotBeNull();
        result!.Id.Should().NotBeNullOrWhiteSpace();
        result.Name.Should().Be(service.Name);
    }

    [Fact]
    public async Task UpdateService_DeveAtualizarServicoERetornar200()
    {
        var token = JwtTokenHelper.GenerateTestToken();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        // Primeiro cria um serviço
        var service = CreateService();
        var serviceResponse = await _client.PostAsJsonAsync("/service", service);
        serviceResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var serviceResult = await serviceResponse.Content.ReadFromJsonAsync<ServiceResponseModel>();
        serviceResult.Should().NotBeNull();

        // Atualiza o serviço
        var updateService = new CreateServiceModel
        {
            Name = "Serviço atualizado"
        };
        var response = await _client.PutAsJsonAsync($"/service/{serviceResult!.Id}", updateService);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateService_DeveRetornar404ParaIdInvalido()
    {
        var token = JwtTokenHelper.GenerateTestToken();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var service = CreateService();
        var response = await _client.PutAsJsonAsync("/service/invalid-id", service);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteService_DeveDeletarServicoERetornar204()
    {
        var token = JwtTokenHelper.GenerateTestToken();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        // Primeiro cria um serviço
        var service = CreateService();
        var serviceResponse = await _client.PostAsJsonAsync("/service", service);
        serviceResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var serviceResult = await serviceResponse.Content.ReadFromJsonAsync<ServiceResponseModel>();
        serviceResult.Should().NotBeNull();

        // Deleta o serviço
        var response = await _client.DeleteAsync($"/service/{serviceResult!.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteService_DeveRetornar404ParaIdInvalido()
    {
        var token = JwtTokenHelper.GenerateTestToken();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await _client.DeleteAsync("/service/invalid-id");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    private CreateServiceModel CreateService()
    {
        var number = _random.Next(1, 20000);
        return new CreateServiceModel
        {
            Name = $"Serviço de teste {number}"
        };
    }

    private class ServiceResponseModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
} 