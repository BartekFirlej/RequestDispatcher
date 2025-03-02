using Request_Dispatcher.Requests;

namespace Request_Dispatcher.Services
{
    public class FlightService : IFlightService
    {
        private readonly ISnowflakeService _snowflakeService;

        public FlightService(ISnowflakeService snowflakeService)
        {
            _snowflakeService = snowflakeService;
        }

        public long BeginFlight(FlightBeginRequest flightBeginRequest)
        {
            return _snowflakeService.NextId();  
        }
    }
}
