using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;

namespace DeliveryApp.Api.Endpoints;

public static class DeliveryEndpoints
{
    public static void MapDeliveryEndpoints(this WebApplication app)
    {
        // GET: /api/orders
        app.MapGet("/api/orders", async (IDeliveryService service) =>
        {
            var all = await service.GetAllAsync();
            return Results.Ok(all);
        });

        // GET: /api/orders/{id}
        app.MapGet("/api/orders/{id:int}", async (int id, IDeliveryService service) =>
        {
            var item = await service.GetByIdAsync(id);
            return item == null ? Results.NotFound() : Results.Ok(item);
        });

        // GET: /api/orders/status/{status}
        app.MapGet("/api/orders/status/{status:int}", async (int status, IDeliveryService service) =>
        {
            var items = await service.GetByStatusAsync(status);
            return Results.Ok(items);
        });

        // POST: /api/orders
        app.MapPost("/api/orders", async (CreateDeliveryDto dto, IDeliveryService service) =>
        {
            var (ok, error, created) = await service.CreateAsync(dto);
            if (!ok) return Results.BadRequest(new { error });
            
            return Results.Created($"/api/orders/{created!.Id}", created);
        });

        // PUT: /api/orders/{id}
        app.MapPut("/api/orders/{id:int}", async (int id, UpdateDeliveryDto dto, IDeliveryService service) =>
        {
            var (ok, error, updated) = await service.UpdateAsync(id, dto);
            if (!ok)
            {
                if (error == "Order not found.") return Results.NotFound();
                return Results.BadRequest(new { error });
            }
            return Results.Ok(updated);
        });

        // DELETE: /api/orders/{id}
        app.MapDelete("/api/orders/{id:int}", async (int id, IDeliveryService service) =>
        {
            var ok = await service.DeleteAsync(id);
            return ok ? Results.NoContent() : Results.NotFound();
        });
    }
}