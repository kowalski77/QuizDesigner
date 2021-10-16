using System;

namespace QuizDesigner.AzureServiceBus
{
    public class MessageProcessor
    {
        public MessageProcessor(string queue, Type type)
        {
            this.Queue = queue;
            this.Type = type;
        }

        public string Queue { get; }

        public Type Type { get; }
    }
}