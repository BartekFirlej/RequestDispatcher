using RabbitMQ.Client;
using Request_Dispatcher.Services.Imlpementations;
using Request_Dispatcher.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

ConnectionFactory factory = new ConnectionFactory();
factory.UserName = "guest";
factory.Password = "guest";
factory.HostName = "localhost";
factory.ClientProvidedName = "app:audit component:event-consumer";

const string SIGNALS_FLIGHT_BEGIN_QUEUE = "Signals_Flight_Begin";
const string SIGNALS_FLIGHT_END_QUEUE = "Signals_Flight_End";
const string SIGNALS_SIGNALS_QUEUE = "Signals_Signals";
const string TARGETS_TARGETS_QUEUE = "Targets_Targets";
const string TARGETS_FLIGHT_BEGIN_QUEUE = "Targets_Flight_Begin";
const string TARGETS_FLIGHT_END_QUEUE = "Targets_Flight_End";

IConnection conn = await factory.CreateConnectionAsync();
IChannel channel = await conn.CreateChannelAsync();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddSingleton<ISnowflakeService>(new SnowflakeService(workerId: 1, datacenterId: 2));
builder.Services.AddSingleton<IRabbitMQPublisherService>(new RabbitMQPublisherService(channel));
builder.Services.AddSingleton<IFlightService>(sp =>
{
    var snowflakeService = sp.GetRequiredService<ISnowflakeService>();
    var rabbitMQPublisherService = sp.GetRequiredService<IRabbitMQPublisherService>();
    return new FlightService(snowflakeService, rabbitMQPublisherService, SIGNALS_FLIGHT_BEGIN_QUEUE, 
        SIGNALS_FLIGHT_END_QUEUE, TARGETS_FLIGHT_BEGIN_QUEUE, TARGETS_FLIGHT_END_QUEUE);
});
builder.Services.AddSingleton<ISignalService>(sp =>
{
    var rabbitMQPublisherService = sp.GetRequiredService<IRabbitMQPublisherService>();
    return new SignalService(rabbitMQPublisherService, SIGNALS_SIGNALS_QUEUE);
});
builder.Services.AddSingleton<ITargetService>(sp =>
{
    var snowflakeService = sp.GetRequiredService<ISnowflakeService>();
    var rabbitMQPublisherService = sp.GetRequiredService<IRabbitMQPublisherService>();
    return new TargetService(snowflakeService, rabbitMQPublisherService, TARGETS_TARGETS_QUEUE);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
