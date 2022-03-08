using Azure.Messaging.ServiceBus;
using GreetingService.Core.Entities;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure.MessageService
{
    public class ServiceBusMessagingService : IMessagingService
    {
        private readonly ServiceBusSender _sender;
        private readonly ServiceBusReceiver _receiver;
        public ServiceBusMessagingService(IConfiguration config)
        {
            string topicName = "main";
            var client = new ServiceBusClient(config["ServiceBusConnectionString"]);
            _sender = client.CreateSender(topicName);
            _receiver = client.CreateReceiver(topicName);
        }

        public async Task SendAsync<T>(T message, MessageSubject subject)
        {
           
            var jsonMessage = JsonSerializer.Serialize(message);
            var encodedMessage = new ServiceBusMessage(jsonMessage);

            encodedMessage.Subject = subject.ToString();



            await _sender.SendMessageAsync(encodedMessage);

            //ServiceBusReceivedMessage receivedMessage = await _receiver.ReceiveMessageAsync();

            //string body = receivedMessage.Body.ToString();
            //Console.WriteLine(body);
        }

    }
}
