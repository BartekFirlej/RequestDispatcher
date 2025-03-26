using RabbitMQ.Client;
using Request_Dispatcher.Services.Imlpementations;
using Request_Dispatcher.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

ConnectionFactory factory = new ConnectionFactory();
factory.UserName = configuration["RabbitMQ:UserName"];
factory.Password = configuration["RabbitMQ:Password"]; 
factory.HostName = configuration["RabbitMQ:HostName"];
factory.ClientProvidedName = configuration["RabbitMQ:ClientProvidedName"];

var queuesSection = builder.Configuration.GetSection("RabbitMQ:Queues");

string SIGNALS_FLIGHT_BEGIN_QUEUE = queuesSection["SignalsFlightBegin"];
string SIGNALS_FLIGHT_END_QUEUE = queuesSection["SignalsFlightEnd"];
string SIGNALS_SIGNALS_QUEUE = queuesSection["SignalsSignals"];
string TARGETS_TARGETS_QUEUE = queuesSection["TargetsTargets"];
string TARGETS_FLIGHT_BEGIN_QUEUE = queuesSection["TargetsFlightBegin"];
string TARGETS_FLIGHT_END_QUEUE = queuesSection["TargetsFlightEnd"];

IConnection conn = await factory.CreateConnectionAsync();
IChannel channel = await conn.CreateChannelAsync();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var snowflakeSection = builder.Configuration.GetSection("Snowflake");
int workerId = int.Parse(snowflakeSection["WorkerId"] ?? "1");
int datacenterId = int.Parse(snowflakeSection["DatacenterId"] ?? "2");
builder.Services.AddSingleton<ISnowflakeService>(new SnowflakeService(workerId: workerId, datacenterId: datacenterId));
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
