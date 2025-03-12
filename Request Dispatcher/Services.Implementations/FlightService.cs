using Request_Dispatcher.Messages;
using Request_Dispatcher.Requests;
using Request_Dispatcher.Services.Interfaces;

namespace Request_Dispatcher.Services.Imlpementations
{
    public class FlightService : IFlightService
    {
        private readonly ISnowflakeService _snowflakeService;
        private readonly IRabbitMQPublisherService _rabbitMQPublisherService;
        private readonly string _SIGNALS_FLIGHT_BEGIN_QUEUE;
        private readonly string _SIGNALS_FLIGHT_END_QUEUE;
        private readonly string _TARGETS_FLIGHT_BEGIN_QUEUE;
        private readonly string _TARGETS_FLIGHT_END_QUEUE;

        public FlightService(ISnowflakeService snowflakeService, IRabbitMQPublisherService rabbitMQPublisherService, 
            string SIGNALS_FLIGHT_BEGIN_QUEUE, string SIGNALS_FLIGHT_END_QUEUE, string TARGETS_FLIGHT_BEGIN_QUEUE, string TARGETS_FLIGHT_END_QUEUE)
        {
            _snowflakeService = snowflakeService;
            _rabbitMQPublisherService = rabbitMQPublisherService;
            _SIGNALS_FLIGHT_BEGIN_QUEUE = SIGNALS_FLIGHT_BEGIN_QUEUE;
            _SIGNALS_FLIGHT_END_QUEUE = SIGNALS_FLIGHT_END_QUEUE;
            _TARGETS_FLIGHT_BEGIN_QUEUE = TARGETS_FLIGHT_BEGIN_QUEUE;
            _TARGETS_FLIGHT_END_QUEUE = TARGETS_FLIGHT_END_QUEUE;
        }

        public long BeginFlight(FlightBeginRequest flightBeginRequest)
        {
            var flightID = _snowflakeService.NextId();
            flightBeginRequest.FlightID = flightID;
            _rabbitMQPublisherService.PublishMessage(flightBeginRequest, _SIGNALS_FLIGHT_BEGIN_QUEUE);
            _rabbitMQPublisherService.PublishMessage(flightBeginRequest, _TARGETS_FLIGHT_BEGIN_QUEUE);
            return flightID;
        }

        public long EndFlight(FlightEndRequest flightEndRequest)
        {
            var flightEndMessage = new FlightEndMessage { FlightID = long.Parse(flightEndRequest.FlightID), EndTime = flightEndRequest.EndTime };
            _rabbitMQPublisherService.PublishMessage(flightEndMessage, _SIGNALS_FLIGHT_END_QUEUE);
            _rabbitMQPublisherService.PublishMessage(flightEndMessage, _TARGETS_FLIGHT_END_QUEUE);
            return flightEndMessage.FlightID;
        }
    }
}
