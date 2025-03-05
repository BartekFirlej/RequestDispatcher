using Request_Dispatcher.Requests;
using Request_Dispatcher.Services.Interfaces;

namespace Request_Dispatcher.Services.Imlpementations
{
    public class TargetService : ITargetService
    {
        private readonly ISnowflakeService _snowflakeService;
        private readonly IRabbitMQPublisherService _rabbitMQPublisherService;
        private readonly string _queueName;

        public TargetService(ISnowflakeService snowflakeService, IRabbitMQPublisherService rabbitMQPublisherService, string queueName)
        {
            _snowflakeService = snowflakeService;
            _rabbitMQPublisherService = rabbitMQPublisherService;
            _queueName = queueName;
        }

        public long ReportTarget(TargetRequest targetRequest)
        {
            if(targetRequest.ObjectId == null)
                targetRequest.ObjectId = _snowflakeService.NextId();
            _rabbitMQPublisherService.PublishMessage(targetRequest, _queueName);
            return targetRequest.ObjectId.Value;
        }
    }
}
