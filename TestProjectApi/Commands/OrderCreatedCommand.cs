namespace TestProjectApi.Commands;

public record OrderCreatedCommand
{
    public Guid OrderId { get; init; }
    public DateTime CreatedAt { get; init; }
}
