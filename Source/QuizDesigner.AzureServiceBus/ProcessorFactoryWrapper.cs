using Azure.Messaging.ServiceBus;

namespace QuizDesigner.AzureServiceBus
{
    public abstract class ProcessorFactoryWrapper
    {
        public abstract ServiceBusProcessor CreateProcessor(string queue);
    }
}