using Request_Dispatcher.Requests;

namespace Request_Dispatcher.Services.Interfaces
{
    public interface ISignalService
    {
        public void SendSignals(SignalRequest signalRequest);
    }
}
