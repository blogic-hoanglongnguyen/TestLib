using System.Reflection;
using BLogic.MassTransit.Enums;
using MassTransit;

namespace BLogic.MassTransit.Options;

public sealed class BLogicPubSubOptions
{
    public TransportKind Transport { get; set; } = TransportKind.InMemory;

    public string Host { get; set; } = "localhost";
    public string VirtualHost { get; set; } = "/";
    public int Port { get; set; } = 5672;
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";

    public Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? ConfigureRabbit { get; set; }
    public Action<IBusRegistrationContext, IInMemoryBusFactoryConfigurator>? ConfigureInMemory { get; set; }

    public Assembly[] ConsumerAssemblies { get; set; } = Array.Empty<Assembly>();
}
