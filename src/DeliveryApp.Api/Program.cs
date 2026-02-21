using Microsoft.EntityFrameworkCore;
// N'oublie pas de créer ces namespaces plus tard pour que ça compile
// using DeliveryApp.Api.Endpoints; 
// using DeliveryApp.Application.Interfaces;
using DeliveryApp.Infrastructure.Data;
// using DeliveryApp.Infrastructure.Repositories;
// using DeliveryApp.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(); // Optionnel, mais recommandé pour tester tes routes

// DbContext — Scoped par défaut via AddDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("DeliveryApp.Infrastructure") // Important pour tes migrations
    )
);

// Enregistrements DI (Injection de Dépendances) — tout en Scoped
// À décommenter au fur et à mesure que tu crées tes interfaces et services
// builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
// builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

// Route de santé (Health Check)
app.MapGet("/health", () => Results.Ok(new { status = "DeliveryApp is running", timestamp = DateTime.UtcNow }));

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.MapOrderEndpoints(); // Extension pour tes routes de commandes

app.Run();