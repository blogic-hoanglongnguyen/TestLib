using System.Reflection;
using System.Security.Authentication;
using BLogic.MassTransit.Enums;
using BLogic.MassTransit.Implementations;
using BLogic.MassTransit.Interfaces;
using BLogic.MassTransit.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BLogic.MassTransit.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add BLogic Pub/Sub with either InMemory or RabbitMQ transport. Automatically scans and registers consumers.
    /// </summary>
    public static IServiceCollection AddBLogicPubSub(this IServiceCollection services, Action<BLogicPubSubOptions> configure)
    {
        services.AddOptions<BLogicPubSubOptions>().Configure(configure);
        services.AddScoped<IEventPublisher, MassTransitEventPublisher>();
        
        services.AddMassTransit((IBusRegistrationConfigurator cfg) =>
        {
            using var sp = services.BuildServiceProvider();
            var opts = sp.GetRequiredService<IOptions<BLogicPubSubOptions>>().Value;

            // Register consumers from provided assemblies
            var consumerAssemblies = opts.ConsumerAssemblies.Length > 0
                ? opts.ConsumerAssemblies
                : new[] { Assembly.GetEntryAssembly()! };

            foreach (var asm in consumerAssemblies)
            {
                cfg.AddConsumers(asm);
            }

            switch (opts.Transport)
            {
                case TransportKind.InMemory:
                    cfg.UsingInMemory((context, busCfg) =>
                    {
                        busCfg.ConfigureEndpoints(context);
                        opts.ConfigureInMemory?.Invoke(context, busCfg);
                    });
                    break;

                case TransportKind.RabbitMq:
                    cfg.UsingRabbitMq((context, busCfg) =>
                    {
                        busCfg.Host(host: opts.Host, port: (ushort)opts.Port, virtualHost: opts.VirtualHost, h =>
                        {
                            h.Username(opts.Username);
                            h.Password(opts.Password);
                        });

                        busCfg.ConfigureEndpoints(context);
                        opts.ConfigureRabbit?.Invoke(context, busCfg);
                    });
                    break;
            }
        });

        return services;
    }
    
    /// <summary>
    /// Convenience helper to set assemblies to scan using a marker type.
    /// </summary>
    public static BLogicPubSubOptions WithConsumersFrom<TMarker>(this BLogicPubSubOptions options)
    {
        options.ConsumerAssemblies = new[] { typeof(TMarker).Assembly };
        return options;
    }
}