using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using QuizCreatedEvents;
using QuizDesigner.Application.Messaging;

namespace QuizDesigner.AzureServiceBus
{
    public sealed class AzureServiceBusPublisher : IMessagePublisher
    {
        private static readonly ConcurrentDictionary<Type, ServiceBusSender> ServiceBusSender = new();

        private readonly ServiceBusClient serviceBusClient;

        public AzureServiceBusPublisher(IOptions<AzureServiceBusOptions> options)
        {
            _ = options?.Value ?? throw new ArgumentNullException(nameof(options));

            this.serviceBusClient = new ServiceBusClient(options.Value.NameSpaceConnectionString);
        }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Azure Queue naming conventions")]
        public async Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            if (integrationEvent == null)
            {
                throw new ArgumentNullException(nameof(integrationEvent));
            }

            var serviceBusSender = ServiceBusSender.GetOrAdd(integrationEvent.GetType(), type => this.serviceBusClient.CreateSender(type.Name.ToLowerInvariant()));

            var serializedIntegrationEvent = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType());
            await serviceBusSender.SendMessageAsync(new ServiceBusMessage(serializedIntegrationEvent), cancellationToken).ConfigureAwait(true);
        }
    }
}