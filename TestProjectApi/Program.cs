using BLogic.Logging.DependencyInjection.Extensions;
using BLogic.Logging.Middlewares;
using BLogic.MassTransit.DependencyInjection.Extensions;
using BLogic.MassTransit.Enums;
using TestProjectApi.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddBLogicLogging();
builder.Host.AddBLogicHostLogging();

builder.Services.AddBLogicPubSub(o =>
{
    o.Transport = TransportKind.InMemory; 
    // o.Transport = TransportKind.RabbitMq;
    //
    // o.Host = builder.Configuration["RabbitMq:Host"];
    // o.Port = int.TryParse(builder.Configuration["RabbitMq:Port"], out var port ) ? port : 5672;
    // o.VirtualHost = builder.Configuration["RabbitMq:VirtualHost"];
    // o.Username = builder.Configuration["RabbitMq:Username"];
    // o.Password = builder.Configuration["RabbitMq:Password"];

    // Scan current assembly for consumers
    o.WithConsumersFrom<OrderCreatedConsumer>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<SerilogMiddleware>();
app.MapControllers();
app.Run();