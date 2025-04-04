﻿using Request_Dispatcher.Requests;
using Request_Dispatcher.Services.Interfaces;

namespace Request_Dispatcher.Services.Imlpementations
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
