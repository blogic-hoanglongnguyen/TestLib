using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BLogicCodeBase.Swagger.Filters;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var info = new OpenApiInfo()
            {
                Title = $"Blogic Dashboard Api Version {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "An Blogic Dashboard Api with versioning",
            };

            if (description.IsDeprecated)
            {
                info.Description += " (Deprecated)";
            }
            
            options.SwaggerDoc(description.GroupName, info);
        }
    }
}