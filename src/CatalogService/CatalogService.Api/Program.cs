using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using CatalogService.Api.Data;


var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string (desde appsettings o env var: ConnectionStrings__Northwind)
var conn = builder.Configuration.GetConnectionString("Northwind");
if (string.IsNullOrWhiteSpace(conn))
    throw new InvalidOperationException("Missing connection string 'ConnectionStrings:Northwind'.");

builder.Services.AddDbContextPool<NorthwindDbContext>(opt =>
{
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));

    opt.UseMySql(conn, serverVersion, mysql =>
    {
        mysql.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        );
    });

    // logs en dev
    if (builder.Environment.IsDevelopment())
        opt.EnableSensitiveDataLogging();
});

// Health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<NorthwindDbContext>("db");

// Pipeline
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints
app.MapHealthChecks("/health", new()
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.MapControllers();

app.Run();
