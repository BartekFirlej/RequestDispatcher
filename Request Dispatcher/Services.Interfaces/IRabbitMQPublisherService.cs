namespace Request_Dispatcher.Services.Interfaces
{
    public interface IRabbitMQPublisherService
    {
        public void PublishMessage<T>(T message, string queueName);
    }
}
