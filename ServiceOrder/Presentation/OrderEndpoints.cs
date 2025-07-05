using ServiceOrder.Application.Models;
using ServiceOrder.Domain.Interfaces;

namespace ServiceOrder.Presentation;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/order", async (IOrderService orderService) =>
        {
            var result = await orderService.GetAllOrdersAsync();
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { errors = result.Errors });
        });

        routes.MapGet("/order/{id}", async (string id, IOrderService orderService) =>
        {
            var result = await orderService.GetOrderByIdAsync(id);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Errors.First() });
        });

        routes.MapPost("/order", async (CreateOrderModel order, IOrderService orderService) =>
        {
            var result = await orderService.CreateOrderAsync(order);

            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Created($"/order/{result.Value.Id}", result.Value);
        });

        routes.MapPut("/order/{id}", async (string id, CreateOrderModel order, IOrderService orderService) =>
        {
            var result = await orderService.UpdateOrderAsync(id, order);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Ok(result.Value);
        });

        routes.MapDelete("/order/{id}", async (string id, IOrderService orderService) =>
        {
            var result = await orderService.DeleteOrderAsync(id);
            return result.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(new { error = result.Errors.First() });
        });
    }
}
