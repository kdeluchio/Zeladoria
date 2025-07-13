using ServiceOrder.Application.Models;
using System.Net.Http.Json;

namespace ServiceOrder.IntegratedTests;

public class OrderEndpointsTests : IClassFixture<ServiceOrderApiFactory>
{
    private readonly ServiceOrderApiFactory _factory;
    private readonly HttpClient _client;
    private readonly Random _random;

    public OrderEndpointsTests(ServiceOrderApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _random = new Random();
    }

    [Fact]
    public async Task GetAllOrders_DeveRetornar200()
    {
        var response = await _client.GetAsync("/order");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetOrderById_DeveRetornar200ParaIdValido()
    {
        // Primeiro cria um serviço
        var service = CreateService();
        var serviceResponse = await _client.PostAsJsonAsync("/service", service);
        serviceResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var serviceResult = await serviceResponse.Content.ReadFromJsonAsync<ServiceResponseModel>();
        serviceResult.Should().NotBeNull();

        // Depois cria um pedido
        var order = CreateOrder(serviceResult!.Id);
        var orderResponse = await _client.PostAsJsonAsync("/order", order);
        orderResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var orderResult = await orderResponse.Content.ReadFromJsonAsync<OrderResponseModel>();
        orderResult.Should().NotBeNull();

        // Busca o pedido criado
        var getResponse = await _client.GetAsync($"/order/{orderResult!.Id}");
        getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetOrderById_DeveRetornar404ParaIdInvalido()
    {
        var response = await _client.GetAsync("/order/invalid-id");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateOrder_DeveCriarPedidoERetornar201()
    {
        // Primeiro cria um serviço
        var service = CreateService();
        var serviceResponse = await _client.PostAsJsonAsync("/service", service);
        serviceResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var serviceResult = await serviceResponse.Content.ReadFromJsonAsync<ServiceResponseModel>();
        serviceResult.Should().NotBeNull();

        // Depois cria um pedido
        var order = CreateOrder(serviceResult!.Id);
        var response = await _client.PostAsJsonAsync("/order", order);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<OrderResponseModel>();
        result.Should().NotBeNull();
        result!.Id.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task UpdateOrder_DeveAtualizarPedidoERetornar200()
    {
        // Primeiro cria um serviço
        var service = CreateService();
        var serviceResponse = await _client.PostAsJsonAsync("/service", service);
        serviceResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var serviceResult = await serviceResponse.Content.ReadFromJsonAsync<ServiceResponseModel>();
        serviceResult.Should().NotBeNull();

        // Depois cria um pedido
        var order = CreateOrder(serviceResult!.Id);
        var orderResponse = await _client.PostAsJsonAsync("/order", order);
        orderResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var orderResult = await orderResponse.Content.ReadFromJsonAsync<OrderResponseModel>();
        orderResult.Should().NotBeNull();

        // Atualiza o pedido
        var updateOrder = CreateOrder(serviceResult.Id);
        updateOrder.Description = "Descrição atualizada";
        var response = await _client.PutAsJsonAsync($"/order/{orderResult!.Id}", updateOrder);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateOrder_DeveRetornar404ParaIdInvalido()
    {
        var service = CreateService();
        var order = CreateOrder("invalid-service-id");
        var response = await _client.PutAsJsonAsync("/order/invalid-id", order);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteOrder_DeveDeletarPedidoERetornar204()
    {
        // Primeiro cria um serviço
        var service = CreateService();
        var serviceResponse = await _client.PostAsJsonAsync("/service", service);
        serviceResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var serviceResult = await serviceResponse.Content.ReadFromJsonAsync<ServiceResponseModel>();
        serviceResult.Should().NotBeNull();

        // Depois cria um pedido
        var order = CreateOrder(serviceResult!.Id);
        var orderResponse = await _client.PostAsJsonAsync("/order", order);
        orderResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var orderResult = await orderResponse.Content.ReadFromJsonAsync<OrderResponseModel>();
        orderResult.Should().NotBeNull();

        // Deleta o pedido
        var response = await _client.DeleteAsync($"/order/{orderResult!.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteOrder_DeveRetornar404ParaIdInvalido()
    {
        var response = await _client.DeleteAsync("/order/invalid-id");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    private CreateOrderModel CreateOrder(string serviceId)
    {
        var number = _random.Next(1, 20000);
        return new CreateOrderModel
        {
            ServiceId = serviceId,
            Description = $"Pedido de teste {number}",
            Address = $"Rua Teste {number}",
            NumberAddress = $"{number}",
            Latitude = -23.5505,
            Longitude = -46.6333
        };
    }

    private CreateServiceModel CreateService()
    {
        var number = _random.Next(1, 20000);
        return new CreateServiceModel
        {
            Name = $"Serviço de teste {number}"
        };
    }

    private class OrderResponseModel
    {
        public string? Id { get; set; }
        public string? ServiceId { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? NumberAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    private class ServiceResponseModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
} 