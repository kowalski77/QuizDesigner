using Azure.Messaging.ServiceBus;

namespace QuizDesigner.AzureServiceBus
{
    public interface IServiceBusClientFactory
    {
        ServiceBusClient Client { get; }
    }
}