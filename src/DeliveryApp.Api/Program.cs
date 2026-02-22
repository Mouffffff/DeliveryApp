using System.Text.Json.Serialization;
using DeliveryApp.Api.Endpoints;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Infrastructure.Data;
using DeliveryApp.Infrastructure.Repositories;
using DeliveryApp.Modules.Catalog.Infrastructure;
using DeliveryApp.Modules.Dispatch.Infrastructure;
using DeliveryApp.Modules.Ordering.Infrastructure;
using DeliveryApp.Modules.Payments.Infrastructure;
using DeliveryApp.Service;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.MigrationsAssembly("DeliveryApp.Infrastructure")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICourierRepository, CourierRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICourierService, CourierService>();
builder.Services.AddScoped<IPaymentAppService, PaymentAppService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddCatalogModule();
builder.Services.AddOrderingModule();
builder.Services.AddDispatchModule();
builder.Services.AddPaymentsModule();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new
{
    status = "DeliveryApp is running",
    timestamp = DateTime.UtcNow
}));

app.MapDeliveryEndpoints();
app.Run();

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception while processing request {Path}", httpContext.Request.Path);

        var result = Results.Problem(
            title: "Unexpected server error",
            detail: "Une erreur interne est survenue.",
            statusCode: StatusCodes.Status500InternalServerError,
            extensions: new Dictionary<string, object?>
            {
                ["traceId"] = httpContext.TraceIdentifier
            });

        await result.ExecuteAsync(httpContext);
        return true;
    }
}
