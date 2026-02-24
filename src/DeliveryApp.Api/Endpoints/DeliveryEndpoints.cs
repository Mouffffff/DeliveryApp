using DeliveryApp.Api.Security;
using DeliveryApp.Application.Common;
using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Domain.Enums;
using DeliveryApp.Modules.Catalog.Application.Abstractions;
using DeliveryApp.Modules.Dispatch.Application.Abstractions;
using DeliveryApp.Modules.Ordering.Application.Abstractions;
using DeliveryApp.Modules.Payments.Application.Abstractions;

namespace DeliveryApp.Api.Endpoints;

public static class DeliveryEndpoints
{
    public static void MapDeliveryEndpoints(this WebApplication app)
    {
        app.MapGet("/api/customers", async (ICustomerService service) =>
        {
            var customers = await service.GetCustomersAsync();
            return Results.Ok(customers);
        }).RequireAuthorization("AdminOnly");

        app.MapPost("/api/addresses", async (CreateAddressDto dto, ICustomerService service) =>
        {
            var result = await service.CreateAddressAsync(dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/addresses/{result.Value!.Id}", result.Value);
        }).RequireAuthorization("AdminOnly");

        app.MapGet("/api/addresses", async (ICustomerService service) =>
        {
            var addresses = await service.GetAddressesAsync();
            return Results.Ok(addresses);
        }).RequireAuthorization("AdminOnly");

        app.MapGet("/api/account/context", async (HttpContext httpContext, ICustomerRepository customers, ICourierRepository couriers) =>
        {
            var role = httpContext.User.GetRole();
            if (role is null)
            {
                return Results.Unauthorized();
            }

            var customerId = httpContext.User.GetCustomerId();
            var courierId = httpContext.User.GetCourierId();

            var response = new
            {
                role = role.ToString(),
                customerId,
                courierId,
                customer = default(object),
                addresses = Array.Empty<object>(),
                courier = default(object)
            };

            if (role == AppRole.Customer && customerId.HasValue)
            {
                var customer = (await customers.GetCustomersAsync()).FirstOrDefault(c => c.Id == customerId.Value);
                var addresses = (await customers.GetAddressesAsync())
                    .Where(a => a.CustomerId == customerId.Value)
                    .Select(a => new { a.Id, a.Street, a.City, a.ZipCode, a.CustomerId })
                    .ToList();

                return Results.Ok(new
                {
                    role = role.ToString(),
                    customerId,
                    courierId = (int?)null,
                    customer = customer is null
                        ? null
                        : new { customer.Id, customer.FullName, customer.Email, customer.PhoneNumber },
                    addresses,
                    courier = (object?)null
                });
            }

            if (role == AppRole.Courier && courierId.HasValue)
            {
                var courier = await couriers.GetCourierByIdAsync(courierId.Value);
                return Results.Ok(new
                {
                    role = role.ToString(),
                    customerId = (int?)null,
                    courierId,
                    customer = (object?)null,
                    addresses = Array.Empty<object>(),
                    courier = courier is null
                        ? null
                        : new { courier.Id, courier.FullName, courier.PhoneNumber, vehicle = courier.Vehicle.ToString(), courier.IsAvailable }
                });
            }

            return Results.Ok(response);
        }).RequireAuthorization();

        app.MapGet("/api/account/addresses", async (HttpContext httpContext, ICustomerRepository customers) =>
        {
            var customerId = httpContext.User.GetCustomerId();
            if (!customerId.HasValue)
            {
                return Results.Forbid();
            }

            var addresses = (await customers.GetAddressesAsync())
                .Where(a => a.CustomerId == customerId.Value)
                .Select(a => new AddressDto(a.Id, a.Street, a.City, a.ZipCode, a.CustomerId))
                .OrderBy(a => a.Id)
                .ToList();

            return Results.Ok(addresses);
        }).RequireAuthorization("CustomerOnly");

        app.MapPost("/api/account/addresses", async (CreateAddressDto dto, HttpContext httpContext, ICustomerService service) =>
        {
            var customerId = httpContext.User.GetCustomerId();
            if (!customerId.HasValue)
            {
                return Results.Forbid();
            }

            dto = dto with { CustomerId = customerId.Value };
            var result = await service.CreateAddressAsync(dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/account/addresses/{result.Value!.Id}", result.Value);
        }).RequireAuthorization("CustomerOnly");

        app.MapPost("/api/stores", async (CreateStoreDto dto, ICatalogService service) =>
        {
            var result = await service.CreateStoreAsync(dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/stores/{result.Value!.Id}", result.Value);
        }).RequireAuthorization("AdminOnly");

        app.MapPut("/api/stores/{storeId:int}", async (int storeId, UpdateStoreDto dto, ICatalogService service) =>
        {
            var result = await service.UpdateStoreAsync(storeId, dto);
            return ToHttpResult(result);
        }).RequireAuthorization("AdminOnly");

        app.MapDelete("/api/stores/{storeId:int}", async (int storeId, ICatalogService service) =>
        {
            var result = await service.DeleteStoreAsync(storeId);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.NoContent();
        }).RequireAuthorization("AdminOnly");

        app.MapPost("/api/products", async (CreateProductDto dto, ICatalogService service) =>
        {
            var result = await service.CreateProductAsync(dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/products/{result.Value!.Id}", result.Value);
        }).RequireAuthorization("AdminOnly");

        app.MapGet("/api/couriers", async (ICourierService service) =>
        {
            var couriers = await service.GetCouriersAsync();
            return Results.Ok(couriers);
        }).RequireAuthorization("CourierOrAdmin");

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
        }).RequireAuthorization("CourierOrAdmin");

        app.MapGet("/api/modules/catalog/stores", async (ICatalogQueries catalog) =>
        {
            var stores = await catalog.GetStoresAsync();
            return Results.Ok(stores);
        }).RequireAuthorization("AdminOnly");

        app.MapGet("/api/modules/catalog/stores/{storeId:int}/products", async (int storeId, ICatalogQueries catalog) =>
        {
            var products = await catalog.GetProductsAsync(storeId);
            return Results.Ok(products);
        }).RequireAuthorization("AdminOnly");

        app.MapPost("/api/modules/ordering/orders", async (PlaceOrderRequest request, IOrderingService ordering) =>
        {
            var result = await ordering.PlaceOrderAsync(request);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
        }).RequireAuthorization("CustomerOnly");

        app.MapGet("/api/modules/ordering/orders/{orderId:int}", async (int orderId, IOrderingService ordering) =>
        {
            var result = await ordering.GetOrderByIdAsync(orderId);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(new { error = result.Error });
        }).RequireAuthorization();

        app.MapPatch("/api/modules/dispatch/orders/{orderId:int}/couriers/{courierId:int}", async (int orderId, int courierId, IDispatchService dispatch) =>
        {
            var result = await dispatch.AssignCourierAsync(orderId, courierId);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
        }).RequireAuthorization("CourierOnly");

        app.MapGet("/api/modules/dispatch/couriers/available", async (IDispatchService dispatch) =>
        {
            var result = await dispatch.GetAvailableCouriersAsync();
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
        }).RequireAuthorization("CourierOnly");

        app.MapPost("/api/modules/payments/pay", async (PayOrderRequest request, IPaymentService payments) =>
        {
            var result = await payments.PayOrderAsync(request);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
        }).RequireAuthorization("CustomerOnly");

        app.MapGet("/api/modules/payments/orders/{orderId:int}", async (int orderId, IPaymentService payments) =>
        {
            var result = await payments.GetOrderPaymentsAsync(orderId);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
        }).RequireAuthorization();

        app.MapGet("/api/orders/mine", async (HttpContext httpContext, IOrderService service) =>
        {
            var role = httpContext.User.GetRole();
            if (role is null)
            {
                return Results.Unauthorized();
            }

            var all = await service.GetAllAsync();
            if (role == AppRole.Admin)
            {
                return Results.Ok(all);
            }

            if (role == AppRole.Customer)
            {
                var customerId = httpContext.User.GetCustomerId();
                if (!customerId.HasValue)
                {
                    return Results.Forbid();
                }

                return Results.Ok(all.Where(o => o.CustomerId == customerId.Value).ToList());
            }

            return Results.Ok(all);
        }).RequireAuthorization();

        app.MapGet("/api/orders", async (IOrderService service) =>
        {
            var all = await service.GetAllAsync();
            return Results.Ok(all);
        }).RequireAuthorization("CourierOrAdmin");

        app.MapGet("/api/orders/{id:int}", async (int id, HttpContext httpContext, IOrderService service) =>
        {
            var result = await service.GetByIdAsync(id);
            if (!result.IsSuccess || result.Value is null)
            {
                return ToHttpResult(result);
            }

            if (!CanAccessOrder(httpContext, result.Value))
            {
                return Results.Forbid();
            }

            return Results.Ok(result.Value);
        }).RequireAuthorization();

        app.MapGet("/api/orders/status/{status:int}", async (int status, IOrderService service) =>
        {
            var result = await service.GetByStatusAsync(status);
            return ToHttpResult(result);
        }).RequireAuthorization("CourierOrAdmin");

        app.MapPost("/api/orders", async (CreateDeliveryDto dto, HttpContext httpContext, IOrderService service, ICustomerRepository customers) =>
        {
            var role = httpContext.User.GetRole();
            if (role == AppRole.Customer)
            {
                var customerId = httpContext.User.GetCustomerId();
                if (!customerId.HasValue)
                {
                    return Results.Forbid();
                }

                dto.CustomerId = customerId.Value;

                var address = (await customers.GetAddressesAsync()).FirstOrDefault(a => a.Id == dto.DeliveryAddressId);
                if (address is null)
                {
                    return Results.Problem(
                        title: "Request failed",
                        detail: "Adresse de livraison introuvable.",
                        statusCode: 404,
                        extensions: new Dictionary<string, object?> { ["errorCode"] = "address_not_found" });
                }

                if (address.CustomerId.HasValue && address.CustomerId.Value != customerId.Value)
                {
                    return Results.Forbid();
                }
            }

            var result = await service.CreateAsync(dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/orders/{result.Value!.Id}", result.Value);
        }).RequireAuthorization("CustomerOnly");

        app.MapPut("/api/orders/{id:int}", async (int id, UpdateDeliveryDto dto, HttpContext httpContext, IOrderService service) =>
        {
            var role = httpContext.User.GetRole();
            if (role == AppRole.Courier)
            {
                var courierId = httpContext.User.GetCourierId();
                if (courierId.HasValue)
                {
                    dto = dto with { CourierId = courierId.Value };
                }
            }

            var result = await service.UpdateAsync(id, dto);
            return ToHttpResult(result);
        }).RequireAuthorization("CourierOnly");

        app.MapPost("/api/orders/{orderId:int}/payments", async (int orderId, CreatePaymentDto dto, HttpContext httpContext, IOrderService orders, IPaymentAppService service) =>
        {
            if (!await EnsureOrderAccessAsync(orderId, httpContext, orders))
            {
                return Results.Forbid();
            }

            var result = await service.CreatePaymentAsync(orderId, dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/orders/{orderId}/payments/{result.Value!.Id}", result.Value);
        }).RequireAuthorization("CustomerOnly");

        app.MapGet("/api/orders/{orderId:int}/payments", async (int orderId, HttpContext httpContext, IOrderService orders, IPaymentAppService service) =>
        {
            if (!await EnsureOrderAccessAsync(orderId, httpContext, orders))
            {
                return Results.Forbid();
            }

            var result = await service.GetOrderPaymentsAsync(orderId);
            return ToHttpResult(result);
        }).RequireAuthorization();

        app.MapPost("/api/orders/{orderId:int}/reviews", async (int orderId, CreateReviewDto dto, HttpContext httpContext, IOrderService orders, IReviewService service) =>
        {
            if (!await EnsureOrderAccessAsync(orderId, httpContext, orders))
            {
                return Results.Forbid();
            }

            var result = await service.CreateOrderReviewAsync(orderId, dto);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.Created($"/api/orders/{orderId}/reviews/{result.Value!.Id}", result.Value);
        }).RequireAuthorization("CustomerOnly");

        app.MapGet("/api/orders/{orderId:int}/reviews", async (int orderId, HttpContext httpContext, IOrderService orders, IReviewService service) =>
        {
            if (!await EnsureOrderAccessAsync(orderId, httpContext, orders))
            {
                return Results.Forbid();
            }

            var result = await service.GetOrderReviewsAsync(orderId);
            return ToHttpResult(result);
        }).RequireAuthorization();

        app.MapPatch("/api/orders/{orderId:int}/assign-courier", async (int orderId, AssignCourierDto dto, HttpContext httpContext, ICourierService service) =>
        {
            var role = httpContext.User.GetRole();
            if (role == AppRole.Courier)
            {
                var courierId = httpContext.User.GetCourierId();
                if (!courierId.HasValue)
                {
                    return Results.Forbid();
                }

                dto = dto with { CourierId = courierId.Value };
            }

            var result = await service.AssignCourierAsync(orderId, dto.CourierId);
            return ToHttpResult(result);
        }).RequireAuthorization("CourierOnly");

        app.MapDelete("/api/orders/{id:int}", async (int id, IOrderService service) =>
        {
            var result = await service.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return ToHttpResult(result);
            }

            return Results.NoContent();
        }).RequireAuthorization("AdminOnly");
    }

    private static bool CanAccessOrder(HttpContext httpContext, DeliveryDto order)
    {
        var role = httpContext.User.GetRole();
        if (role is null)
        {
            return false;
        }

        if (role == AppRole.Admin || role == AppRole.Courier)
        {
            return true;
        }

        if (role == AppRole.Customer)
        {
            var customerId = httpContext.User.GetCustomerId();
            return customerId.HasValue && customerId.Value == order.CustomerId;
        }

        return false;
    }

    private static async Task<bool> EnsureOrderAccessAsync(int orderId, HttpContext httpContext, IOrderService orders)
    {
        var role = httpContext.User.GetRole();
        if (role is null)
        {
            return false;
        }

        if (role == AppRole.Admin || role == AppRole.Courier)
        {
            return true;
        }

        if (role != AppRole.Customer)
        {
            return false;
        }

        var customerId = httpContext.User.GetCustomerId();
        if (!customerId.HasValue)
        {
            return false;
        }

        var order = await orders.GetByIdAsync(orderId);
        return order.IsSuccess && order.Value is not null && order.Value.CustomerId == customerId.Value;
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
