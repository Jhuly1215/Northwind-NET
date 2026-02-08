using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OrdersService.Api.Swagger;

public sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = "OrdersService.Api",
                Version = description.ApiVersion.ToString()
            });
        }

        // Importante: que cada endpoint caiga en su doc por GroupName
        options.DocInclusionPredicate((docName, apiDesc) =>
            string.Equals(apiDesc.GroupName, docName, StringComparison.OrdinalIgnoreCase));
    }
}
