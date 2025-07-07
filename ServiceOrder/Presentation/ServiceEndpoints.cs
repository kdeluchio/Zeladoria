using ServiceOrder.Application.Models;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Infra.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ServiceOrder.Presentation;

public static class ServiceEndpoints
{
    public static void MapServiceEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/service", async (IServiceService serviceService) =>
        {
            var result = await serviceService.GetAllServicesAsync();
            return result.ToHttpResult();
        })
        .RequireAuthorization();

        routes.MapGet("/service/{id}", async (string id, IServiceService serviceService) =>
        {
            var result = await serviceService.GetServiceByIdAsync(id);
            return result.ToHttpResult();
        })
        .RequireAuthorization();

        routes.MapPost("/service", async (CreateServiceModel service, IServiceService serviceService) =>
        {
            var result = await serviceService.CreateServiceAsync(service);
            return result.ToCreatedResult($"/service/{result.Value?.Id}");
        })
        .RequireAuthorization();

        routes.MapPut("/service/{id}", async (string id, CreateServiceModel service, IServiceService serviceService) =>
        {
            var result = await serviceService.UpdateServiceAsync(id, service);
            return result.ToHttpResult();
        })
        .RequireAuthorization();

        routes.MapDelete("/service/{id}", async (string id, IServiceService serviceService) =>
        {
            var result = await serviceService.DeleteServiceAsync(id);
            return result.ToNoContentResult();
        })
        .RequireAuthorization();
    }
}