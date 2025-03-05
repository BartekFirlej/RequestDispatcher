using RabbitMQ.Client;
using Request_Dispatcher.Services;

var builder = WebApplication.CreateBuilder(args);

ConnectionFactory factory = new ConnectionFactory();
factory.UserName = "guest";
factory.Password = "guest";
factory.HostName = "localhost";
factory.ClientProvidedName = "app:audit component:event-consumer";

const string FLIGHTS_QUEUE = "Flights";
const string SIGNALS_QUEUE = "Signals";
const string TARGETS_QUEUE = "Targets";

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
    return new FlightService(snowflakeService, rabbitMQPublisherService, FLIGHTS_QUEUE);
});
builder.Services.AddSingleton<ISignalService>(sp =>
{
    var rabbitMQPublisherService = sp.GetRequiredService<IRabbitMQPublisherService>();
    return new SignalService(rabbitMQPublisherService, SIGNALS_QUEUE);
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
