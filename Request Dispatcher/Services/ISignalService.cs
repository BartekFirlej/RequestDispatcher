using Request_Dispatcher.Requests;

namespace Request_Dispatcher.Services
{
    public interface ISignalService
    {
        public void SendSignals(SignalRequest signalRequest);
    }
}
