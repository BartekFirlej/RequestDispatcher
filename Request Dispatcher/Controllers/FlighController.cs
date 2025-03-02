using Microsoft.AspNetCore.Mvc;
using Request_Dispatcher.Requests;
using Request_Dispatcher.Responses;
using Request_Dispatcher.Services;

namespace Request_Dispatcher.Controllers
{
    [ApiController]
    [Route("flight")]
    public class FlighController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlighController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpPost]
        public FlightBeginResponse BeginFlight(FlightBeginRequest flightBeginRequest)
        {
            var flightId = _flightService.BeginFlight(flightBeginRequest);
            return new FlightBeginResponse { FlightId = flightId };
        }
    }
}
