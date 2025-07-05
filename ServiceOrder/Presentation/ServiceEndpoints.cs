using ServiceOrder.Application.Models;
using ServiceOrder.Domain.Interfaces;

namespace ServiceOrder.Presentation;

public static class ServiceEndpoints
{
    public static void MapServiceEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/service", async (IServiceService serviceService) =>
        {
            var result = await serviceService.GetAllServicesAsync();
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { errors = result.Errors });
        });

        routes.MapGet("/service/{id}", async (string id, IServiceService serviceService) =>
        {
            var result = await serviceService.GetServiceByIdAsync(id);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Errors.First() });
        });

        routes.MapPost("/service", async (CreateServiceModel service, IServiceService serviceService) =>
        {
            var result = await serviceService.CreateServiceAsync(service);

            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Created($"/service/{result.Value.Id}", result.Value);
        });

        routes.MapPut("/service/{id}", async (string id, CreateServiceModel service, IServiceService serviceService) =>
        {
            var result = await serviceService.UpdateServiceAsync(id, service);

            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Ok(result.Value);
        });

        routes.MapDelete("/service/{id}", async (string id, IServiceService serviceService) =>
        {
            var result = await serviceService.DeleteServiceAsync(id);
            return result.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(new { error = result.Errors.First() });
        });
    }
}