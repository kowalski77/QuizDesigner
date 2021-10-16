using System.Collections.Generic;

namespace QuizDesigner.AzureServiceBus
{
    public class ServiceBusOptions
    {
        public string ConnectionString { get; set; } = string.Empty;

        public IEnumerable<MessageProcessor> MessageProcessors { get; set; } = new List<MessageProcessor>();
    }
}