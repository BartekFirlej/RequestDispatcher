using Microsoft.AspNetCore.Mvc;
using Request_Dispatcher.Requests;
using Request_Dispatcher.Services;

namespace Request_Dispatcher.Controllers
{
    [ApiController]
    [Route("signals")]
    public class SignalController : ControllerBase
    {
        private readonly ISignalService _signalService;

        public SignalController(ISignalService signalService)
        {
            _signalService = signalService;
        }

        [HttpPost]
        public IActionResult SendSignal(SignalRequest signalRequest)
        {
            _signalService.SendSignals(signalRequest);
            return Ok();
        }
    }
}
