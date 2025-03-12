using Microsoft.AspNetCore.Mvc;
using Request_Dispatcher.Requests;
using Request_Dispatcher.Responses;
using Request_Dispatcher.Services.Interfaces;

namespace Request_Dispatcher.Controllers
{
    [ApiController]
    [Route("flights")]
    public class FlighController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlighController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpPost("begin")]
        public IActionResult BeginFlight(FlightBeginRequest flightBeginRequest)
        {
            var flightId = _flightService.BeginFlight(flightBeginRequest);
            return CreatedAtAction(nameof(BeginFlight), new FlightBeginResponse { FlightId = flightId.ToString() });
        }

        [HttpPost("end")]
        public IActionResult EndFlight(FlightEndRequest flightEndRequest)
        {
            var flightId = _flightService.EndFlight(flightEndRequest);
            return CreatedAtAction(nameof(BeginFlight), new FlightEndResponse { FlightId = flightId.ToString() });
        }
    }
}
