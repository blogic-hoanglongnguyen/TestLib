using BLogic.MassTransit.Interfaces;
using MassTransit;

namespace BLogic.MassTransit.Implementations;

internal sealed class MassTransitEventPublisher(IPublishEndpoint endpoint) : IEventPublisher
{
    private readonly IPublishEndpoint _endpoint = endpoint;
    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        => _endpoint.Publish(message, cancellationToken);
}