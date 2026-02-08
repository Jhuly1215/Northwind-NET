using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OrdersService.Api.Data;
using OrdersService.Api.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Versioning: /api/v{version}/...
builder.Services
    .AddApiVersioning(o =>
    {
        o.DefaultApiVersion = new ApiVersion(1, 0);
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.ReportApiVersions = true;
        o.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(o =>
    {
        o.GroupNameFormat = "'v'V";          // v1, v2
        o.SubstituteApiVersionInUrl = true; // reemplaza {version:apiVersion}
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// DB
var conn = builder.Configuration.GetConnectionString("Northwind");
if (string.IsNullOrWhiteSpace(conn))
    throw new InvalidOperationException("Missing connection string 'ConnectionStrings:Northwind'.");

builder.Services.AddDbContextPool<OrdersDbContext>(opt =>
{
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 45));
    opt.UseMySql(conn, serverVersion, mysql =>
    {
        mysql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });

    if (builder.Environment.IsDevelopment())
        opt.EnableSensitiveDataLogging();
});

// Health
builder.Services.AddHealthChecks()
    .AddDbContextCheck<OrdersDbContext>("db");

var app = builder.Build();

var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(o =>
    {
        foreach (var desc in apiVersionProvider.ApiVersionDescriptions)
        {
            o.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", $"Orders {desc.GroupName.ToUpperInvariant()}");
        }
        o.RoutePrefix = "swagger"; // /swagger
    });
}

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
