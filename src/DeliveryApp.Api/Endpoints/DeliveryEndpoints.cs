using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Modules.Catalog.Application.Abstractions;
using DeliveryApp.Modules.Dispatch.Application.Abstractions;
using DeliveryApp.Modules.Ordering.Application.Abstractions;
using DeliveryApp.Modules.Payments.Application.Abstractions;

namespace DeliveryApp.Api.Endpoints;

public static class DeliveryEndpoints
{
    public static void MapDeliveryEndpoints(this WebApplication app)
    {
        app.MapPost("/api/customers", async (CreateCustomerDto dto, ICustomerService service) =>
        {
            var result = await service.CreateCustomerAsync(dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/customers/{result.Value!.Id}", result.Value);
        });

        app.MapGet("/api/customers", async (ICustomerService service) =>
        {
            var customers = await service.GetCustomersAsync();
            return Results.Ok(customers);
        });

        app.MapPost("/api/addresses", async (CreateAddressDto dto, ICustomerService service) =>
        {
            var result = await service.CreateAddressAsync(dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/addresses/{result.Value!.Id}", result.Value);
        });

        app.MapGet("/api/addresses", async (ICustomerService service) =>
        {
            var addresses = await service.GetAddressesAsync();
            return Results.Ok(addresses);
        });

        app.MapPost("/api/stores", async (CreateStoreDto dto, ICatalogService service) =>
        {
            var result = await service.CreateStoreAsync(dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/stores/{result.Value!.Id}", result.Value);
        });

        app.MapPost("/api/products", async (CreateProductDto dto, ICatalogService service) =>
        {
            var result = await service.CreateProductAsync(dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/products/{result.Value!.Id}", result.Value);
        });

        app.MapPost("/api/couriers", async (CreateCourierDto dto, ICourierService service) =>
        {
            var result = await service.CreateCourierAsync(dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/couriers/{result.Value!.Id}", result.Value);
        });

        app.MapGet("/api/couriers", async (ICourierService service) =>
        {
            var couriers = await service.GetCouriersAsync();
            return Results.Ok(couriers);
        });

        app.MapGet("/api/stores", async (ICatalogService service) =>
        {
            var stores = await service.GetStoresAsync();
            return Results.Ok(stores);
        });

        app.MapGet("/api/stores/{storeId:int}/products", async (int storeId, ICatalogService service) =>
        {
            var result = await service.GetStoreProductsAsync(storeId);
            return ToHttpResult(result);
        });

        app.MapGet("/api/couriers/available", async (ICourierService service) =>
        {
            var couriers = await service.GetAvailableCouriersAsync();
            return Results.Ok(couriers);
        });

        app.MapGet("/api/modules/catalog/stores", async (ICatalogQueries catalog) =>
        {
            var stores = await catalog.GetStoresAsync();
            return Results.Ok(stores);
        });

        app.MapGet("/api/modules/catalog/stores/{storeId:int}/products", async (int storeId, ICatalogQueries catalog) =>
        {
            var products = await catalog.GetProductsAsync(storeId);
            return Results.Ok(products);
        });

        app.MapPost("/api/modules/ordering/orders", async (PlaceOrderRequest request, IOrderingService ordering) =>
        {
            var result = await ordering.PlaceOrderAsync(request);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
        });

        app.MapGet("/api/modules/ordering/orders/{orderId:int}", async (int orderId, IOrderingService ordering) =>
        {
            var result = await ordering.GetOrderByIdAsync(orderId);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(new { error = result.Error });
        });

        app.MapPatch("/api/modules/dispatch/orders/{orderId:int}/couriers/{courierId:int}", async (int orderId, int courierId, IDispatchService dispatch) =>
        {
            var result = await dispatch.AssignCourierAsync(orderId, courierId);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
        });

        app.MapGet("/api/modules/dispatch/couriers/available", async (IDispatchService dispatch) =>
        {
            var result = await dispatch.GetAvailableCouriersAsync();
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
        });

        app.MapPost("/api/modules/payments/pay", async (PayOrderRequest request, IPaymentService payments) =>
        {
            var result = await payments.PayOrderAsync(request);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
        });

        app.MapGet("/api/modules/payments/orders/{orderId:int}", async (int orderId, IPaymentService payments) =>
        {
            var result = await payments.GetOrderPaymentsAsync(orderId);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
        });

        app.MapGet("/api/orders", async (IOrderService service) =>
        {
            var all = await service.GetAllAsync();
            return Results.Ok(all);
        });

        app.MapGet("/api/orders/{id:int}", async (int id, IOrderService service) =>
        {
            var result = await service.GetByIdAsync(id);
            return ToHttpResult(result);
        });

        app.MapGet("/api/orders/status/{status:int}", async (int status, IOrderService service) =>
        {
            var result = await service.GetByStatusAsync(status);
            return ToHttpResult(result);
        });

        app.MapPost("/api/orders", async (CreateDeliveryDto dto, IOrderService service) =>
        {
            var result = await service.CreateAsync(dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/orders/{result.Value!.Id}", result.Value);
        });

        app.MapPut("/api/orders/{id:int}", async (int id, UpdateDeliveryDto dto, IOrderService service) =>
        {
            var result = await service.UpdateAsync(id, dto);
            return ToHttpResult(result);
        });

        app.MapPost("/api/orders/{orderId:int}/payments", async (int orderId, CreatePaymentDto dto, IPaymentAppService service) =>
        {
            var result = await service.CreatePaymentAsync(orderId, dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/orders/{orderId}/payments/{result.Value!.Id}", result.Value);
        });

        app.MapGet("/api/orders/{orderId:int}/payments", async (int orderId, IPaymentAppService service) =>
        {
            var result = await service.GetOrderPaymentsAsync(orderId);
            return ToHttpResult(result);
        });

        app.MapPost("/api/orders/{orderId:int}/reviews", async (int orderId, CreateReviewDto dto, IReviewService service) =>
        {
            var result = await service.CreateOrderReviewAsync(orderId, dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/orders/{orderId}/reviews/{result.Value!.Id}", result.Value);
        });

        app.MapGet("/api/orders/{orderId:int}/reviews", async (int orderId, IReviewService service) =>
        {
            var result = await service.GetOrderReviewsAsync(orderId);
            return ToHttpResult(result);
        });

        app.MapPatch("/api/orders/{orderId:int}/assign-courier", async (int orderId, AssignCourierDto dto, ICourierService service) =>
        {
            var result = await service.AssignCourierAsync(orderId, dto.CourierId);
            return ToHttpResult(result);
        });

        app.MapDelete("/api/orders/{id:int}", async (int id, IOrderService service) =>
        {
            var result = await service.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.NoContent();
        });
    }

    private static IResult ToHttpResult<T>(ServiceResult<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        var error = result.Error!;
        return Results.Problem(
            title: "Request failed",
            detail: error.Message,
            statusCode: error.StatusCode,
            extensions: new Dictionary<string, object?>
            {
                ["errorCode"] = error.Code
            });
    }
}
