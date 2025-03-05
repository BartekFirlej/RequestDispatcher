using Microsoft.AspNetCore.Mvc;
using Request_Dispatcher.Requests;
using Request_Dispatcher.Responses;
using Request_Dispatcher.Services.Interfaces;

namespace Request_Dispatcher.Controllers
{
    [ApiController]
    [Route("targets")]
    public class TargetController : ControllerBase
    {
        private readonly ITargetService _targetService;

        public TargetController(ITargetService targetService)
        {
            _targetService = targetService;
        }

        [HttpPost]
        public IActionResult ReportTarget(TargetRequest targetRequest)
        {
            var id = _targetService.ReportTarget(targetRequest);
            return CreatedAtAction(nameof(ReportTarget), new TargetResponse { TargetId = id });
        }
    }
}
