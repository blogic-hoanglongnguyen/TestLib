using MassTransit;
using TestProjectApi.Commands;

namespace TestProjectApi.Consumers;

public sealed class OrderCreatedConsumer : IConsumer<OrderCreatedCommand>
{
    public Task Consume(ConsumeContext<OrderCreatedCommand> context)
    {
        Console.WriteLine($"[Consumer] OrderId={context.Message.OrderId}, ts={context.Message.CreatedAt:o}");
        return Task.CompletedTask;
    }
}