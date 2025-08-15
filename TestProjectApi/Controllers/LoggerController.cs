using BLogic.MassTransit.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TestProjectApi.Commands;
using ILogger = Serilog.ILogger;

namespace TestProjectApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoggerController(ILogger logger, IEventPublisher publisher) : ControllerBase
{
    private readonly IEventPublisher publisher = publisher;
    [HttpPost]
    public async Task<IActionResult> PublishAsync()
    {
        await publisher.PublishAsync(new OrderCreatedCommand
        {
            OrderId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        });
        return Ok();
    }
}