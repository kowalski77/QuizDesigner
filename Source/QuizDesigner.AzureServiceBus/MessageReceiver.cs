using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace QuizDesigner.AzureServiceBus
{
    public class MessageReceiver : IAsyncDisposable
    {
        private readonly List<ServiceBusProcessor> serviceBusProcessors = new();
        private readonly IServiceProvider serviceProvider;

        public MessageReceiver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var processor in this.serviceBusProcessors)
            {
                await processor.DisposeAsync().ConfigureAwait(false);
            }
        }

        public void AddProcessor(string queue, Type type)
        {
            var processorFactoryWrapper = this.GetProcessorFactoryWrapper(type);
            if (processorFactoryWrapper is null)
            {
                return;
            }

            this.serviceBusProcessors.Add(processorFactoryWrapper.CreateProcessor(queue));
        }

        public async Task StartAsync()
        {
            foreach (var processor in this.serviceBusProcessors)
            {
                await processor.StartProcessingAsync().ConfigureAwait(false);
            }
        }

        public async Task StopAsync()
        {
            foreach (var processor in this.serviceBusProcessors)
            {
                await processor.StopProcessingAsync().ConfigureAwait(false);
            }
        }

        private ProcessorFactoryWrapper? GetProcessorFactoryWrapper(Type type)
        {
            var processorFactoryWrapper = Activator.CreateInstance(
                    typeof(ProcessorFactory<>).MakeGenericType(type), 
                    this.serviceProvider)
                as ProcessorFactoryWrapper;

            return processorFactoryWrapper;
        }
    }
}