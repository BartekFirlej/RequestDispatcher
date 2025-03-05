using Request_Dispatcher.Requests;

namespace Request_Dispatcher.Services
{
    public class FlightService : IFlightService
    {
        private readonly ISnowflakeService _snowflakeService;
        private readonly IRabbitMQPublisherService _rabbitMQPublisherService;
        private readonly string _queueName;

        public FlightService(ISnowflakeService snowflakeService, IRabbitMQPublisherService rabbitMQPublisherService, string queueName)
        {
            _snowflakeService = snowflakeService;
            _rabbitMQPublisherService = rabbitMQPublisherService;
            _queueName = queueName;
        }

        public long BeginFlight(FlightBeginRequest flightBeginRequest)
        {
            _rabbitMQPublisherService.PublishMessage(flightBeginRequest, _queueName);
            return _snowflakeService.NextId();  
        }
    }
}
