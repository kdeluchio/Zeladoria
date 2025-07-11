using ServiceOrder.Application.Models;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Infra.Extensions;

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

    }
}
