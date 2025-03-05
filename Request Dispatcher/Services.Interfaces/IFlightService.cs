using Request_Dispatcher.Requests;

namespace Request_Dispatcher.Services.Interfaces
{
    public interface IFlightService
    {
        public long BeginFlight(FlightBeginRequest flightBeginRequest);
    }
}
