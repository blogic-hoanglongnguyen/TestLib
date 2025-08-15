using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BLogicCodeBase.Swagger.Attributes;

public class SwaggerIgnoreSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties == null || context?.Type == null) return;

        var ignoredProperties = context.Type.GetProperties()
            .Where(p => p.GetCustomAttribute<SwaggerIgnoreAttribute>() != null)
            .Select(p => char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1)) // match JSON name
            .ToList();

        foreach (var propName in ignoredProperties)
        {
            schema.Properties.Remove(propName);
        }
    }
}