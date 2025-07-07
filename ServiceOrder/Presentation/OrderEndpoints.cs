using ServiceOrder.Application.Models;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Infra.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ServiceOrder.Presentation;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/order", async (IOrderService orderService) =>
        {
            var result = await orderService.GetAllOrdersAsync();
            return result.ToHttpResult();
        })
        .RequireAuthorization();

        routes.MapGet("/order/{id}", async (string id, IOrderService orderService) =>
        {
            var result = await orderService.GetOrderByIdAsync(id);
            return result.ToHttpResult();
        })
        .RequireAuthorization();

        routes.MapPost("/order", async (CreateOrderModel order, IOrderService orderService) =>
        {
            var result = await orderService.CreateOrderAsync(order);
            return result.ToCreatedResult($"/order/{result.Value?.Id}");
        })
        .RequireAuthorization();

        routes.MapPut("/order/{id}", async (string id, CreateOrderModel order, IOrderService orderService) =>
        {
            var result = await orderService.UpdateOrderAsync(id, order);
            return result.ToHttpResult();
        })
        .RequireAuthorization();

        routes.MapDelete("/order/{id}", async (string id, IOrderService orderService) =>
        {
            var result = await orderService.DeleteOrderAsync(id);
            return result.ToNoContentResult();
        })
        .RequireAuthorization();

        // Endpoint de teste para verificar autenticação
        routes.MapGet("/order/test-auth", (ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = user.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;
            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            return Results.Ok(new
            {
                Message = "Autenticação JWT funcionando corretamente!",
                UserId = userId,
                UserName = userName,
                UserEmail = userEmail,
                UserRole = userRole
            });
        })
        .RequireAuthorization();

        // Exemplo de endpoint que acessa ClaimsPrincipal diretamente
        routes.MapGet("/order/my-orders", async (ClaimsPrincipal user, IOrderService orderService) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            // Filtrar pedidos baseado no role do usuário
            var allOrders = await orderService.GetAllOrdersAsync();
            
            if (!allOrders.IsSuccess)
            {
                return Results.Problem(allOrders.Errors.First());
            }

            var filteredOrders = userRole switch
            {
                "Admin" => allOrders.Value, // Administradores veem todos os pedidos
                "User" => allOrders.Value.Where(o => o.Customer?.Id == userId), // Usuários veem apenas seus pedidos
                "Technician" => allOrders.Value.Where(o => o.Technician?.Id == userId), // Técnicos veem apenas pedidos atribuídos
                _ => Enumerable.Empty<OrderResponseModel>()
            };

            return Results.Ok(new
            {
                UserId = userId,
                UserRole = userRole,
                Orders = filteredOrders
            });
        })
        .RequireAuthorization();

        // Exemplo de endpoint que verifica roles específicos
        routes.MapGet("/order/admin/stats", (ClaimsPrincipal user) =>
        {
            // Verificar se o usuário tem role de Admin
            if (!user.IsInRole("Admin"))
            {
                return Results.Forbid();
            }

            var adminId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var adminName = user.FindFirst(ClaimTypes.Name)?.Value;

            return Results.Ok(new
            {
                Message = "Estatísticas administrativas",
                AdminId = adminId,
                AdminName = adminName,
                AccessTime = DateTime.UtcNow
            });
        })
        .RequireAuthorization()
        .RequireAuthorization(policy => policy.RequireRole("Admin"));

    }
}
