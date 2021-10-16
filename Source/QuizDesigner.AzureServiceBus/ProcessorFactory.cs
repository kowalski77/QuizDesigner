using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace QuizDesigner.AzureServiceBus
{
    public sealed class ProcessorFactory<T> : ProcessorFactoryWrapper
    {
        private readonly IServiceBusClientFactory clientFactory;
        private readonly IConsumer<T> consumer;

        public ProcessorFactory(IServiceBusClientFactory clientFactory, IConsumer<T> consumer)
        {
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            this.consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        }

        public override ServiceBusProcessor CreateProcessor(string queue)
        {
            var processor = this.clientFactory.Client.CreateProcessor(queue);

            processor.ProcessMessageAsync += this.OnProcessMessageAsync;
            processor.ProcessErrorAsync += ProcessorOnProcessErrorAsync;

            return processor;
        }

        private async Task OnProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var message = JsonSerializer.Deserialize<T>(arg.Message.Body) ??
                          throw new SerializationException($"Could not deserialize type: {typeof(T)}");

            await this.consumer.Consume(message).ConfigureAwait(false);
        }

        private static Task ProcessorOnProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            return Task.CompletedTask;
        }
    }
}