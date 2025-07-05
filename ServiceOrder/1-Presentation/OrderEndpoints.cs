using ServiceOrder.Application.Interfaces;
using ServiceOrder.Application.Models;

namespace ServiceOrder.Presentation;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/order", async (IOrderService orderService) =>
        {
            var orders = await orderService.GetAllOrdersAsync();
            return Results.Ok(orders);
        });

        routes.MapGet("/order/{id}", async (string id, IOrderService orderService) =>
        {
            var order = await orderService.GetOrderByIdAsync(id);
            return order != null ? Results.Ok(order) : Results.NotFound();
        });

        routes.MapPost("/order", async (CreateOrderModel order, IOrderService orderService) =>
        {
            try
            {
                var createdOrder = await orderService.CreateOrderAsync(order);
                return Results.Created($"/order/{createdOrder.Id}", createdOrder);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        routes.MapPut("/order/{id}", async (string id, CreateOrderModel order, IOrderService orderService) =>
        {
            try
            {
                var updatedOrder = await orderService.UpdateOrderAsync(id, order);
                return Results.Ok(updatedOrder);
            }
            catch (ArgumentException)
            {
                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        routes.MapDelete("/order/{id}", async (string id, IOrderService orderService) =>
        {
            var deleted = await orderService.DeleteOrderAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
