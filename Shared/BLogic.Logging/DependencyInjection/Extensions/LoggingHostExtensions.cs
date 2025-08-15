using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace BLogic.Logging.DependencyInjection.Extensions;

public static class LoggingHostExtensions
{
    public static void AddBLogicHostLogging(this IHostBuilder hostBuilder)
    {
        // hostBuilder.UseSerilog((ctx, lc) =>
        //     lc.ReadFrom.Configuration(ctx.Configuration));
        hostBuilder.UseSerilog((ctx, lc) =>
        {
            lc.Enrich.FromLogContext().ReadFrom.Configuration(ctx.Configuration);

            var esSection = ctx.Configuration
                .GetSection("Serilog:WriteTo")
                .GetChildren()
                .FirstOrDefault(s =>
                    string.Equals(s["Name"], "Elasticsearch", StringComparison.OrdinalIgnoreCase));

            if (esSection is null)
                return;
            
            var args = esSection.GetSection("Args");
            var nodeUris = args["nodeUris"];
            var indexFormat = args["indexFormat"];
            var autoRegister = bool.TryParse(args["autoRegisterTemplate"], out var b) && b;

            var ba = args.GetSection("basicAuthentication");
            var username = ba["username"];
            var password = ba["password"];

            lc.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(nodeUris))
            {
                AutoRegisterTemplate = true,
                IndexFormat = indexFormat,
                ModifyConnectionSettings = x => x.BasicAuthentication(username, password)
            });
        });
    }
}