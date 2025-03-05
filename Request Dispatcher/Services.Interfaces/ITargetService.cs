using Request_Dispatcher.Requests;

namespace Request_Dispatcher.Services.Interfaces
{
    public interface ITargetService
    {
        public long ReportTarget(TargetRequest targetRequest);
    }
}
