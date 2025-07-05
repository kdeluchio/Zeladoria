using ServiceOrder.Application.Interfaces;
using ServiceOrder.Application.Models;

namespace ServiceOrder.Presentation;

public static class ServiceEndpoints
{
    public static void MapServiceEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/service", async (IServiceService serviceService) =>
        {
            var services = await serviceService.GetAllServicesAsync();
            return Results.Ok(services);
        });

        routes.MapGet("/service/{id}", async (string id, IServiceService serviceService) =>
        {
            var service = await serviceService.GetServiceByIdAsync(id);
            return service != null ? Results.Ok(service) : Results.NotFound();
        });

        routes.MapPost("/service", async (CreateServiceModel service, IServiceService serviceService) =>
        {
            try
            {
                var createdService = await serviceService.CreateServiceAsync(service);
                return Results.Created($"/service/{createdService.Id}", createdService);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        routes.MapPut("/service/{id}", async (string id, CreateServiceModel service, IServiceService serviceService) =>
        {
            try
            {
                var updatedService = await serviceService.UpdateServiceAsync(id, service);
                return Results.Ok(updatedService);
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

        routes.MapDelete("/service/{id}", async (string id, IServiceService serviceService) =>
        {
            var deleted = await serviceService.DeleteServiceAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
} 