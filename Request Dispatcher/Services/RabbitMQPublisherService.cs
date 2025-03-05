using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace Request_Dispatcher.Services
{
    public class RabbitMQPublisherService : IRabbitMQPublisherService
    {
        private readonly IChannel _channel;

        public RabbitMQPublisherService(IChannel channel)
        {
            _channel = channel;
        }

        public async void PublishMessage<T>(T message, string queueName)
        {
            try
            {
                var serializedMessage = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(serializedMessage);
                await _channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error publishing message to RabbitMQ: {ex.Message}");
            }
        }
    }
}
