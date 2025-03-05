using Request_Dispatcher.Requests;

namespace Request_Dispatcher.Services
{
    public class SignalService : ISignalService
    {
        private readonly IRabbitMQPublisherService _rabbitMQPublisherService;
        private readonly string _queueName;

        public SignalService(IRabbitMQPublisherService rabbitMQPublisherService, string queueName)
        {
            _rabbitMQPublisherService = rabbitMQPublisherService;
            _queueName = queueName;
        }

        public void SendSignals(SignalRequest signalRequest)
        {
            _rabbitMQPublisherService.PublishMessage(signalRequest, _queueName);
        }
    }
}
