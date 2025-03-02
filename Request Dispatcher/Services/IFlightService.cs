using Request_Dispatcher.Requests;

namespace Request_Dispatcher.Services
{
    public interface IFlightService
    {
        public long BeginFlight(FlightBeginRequest flightBeginRequest);
    }
}
