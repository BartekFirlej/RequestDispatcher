namespace Request_Dispatcher.Services
{
    public interface IRabbitMQPublisherService
    {
        public void PublishMessage<T>(T message, string queueName);
    }
}
