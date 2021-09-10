using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using QuizCreatedEvents;

namespace QuizDesigner.OutboxSender
{
    public sealed class AzureStorageQueuePublisher : IMessagePublisher
    {
        private static readonly ConcurrentDictionary<Type, QueueClient> QueueClients = new();

        private readonly string storageConnectionString;

        public AzureStorageQueuePublisher(string storageConnectionString)
        {
            this.storageConnectionString = storageConnectionString;
        }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Azure Queue naming conventions")]
        public async Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            if (integrationEvent == null)
            {
                throw new ArgumentNullException(nameof(integrationEvent));
            }

            var queueClient = QueueClients.GetOrAdd(integrationEvent.GetType(), type => new QueueClient(this.storageConnectionString, type.Name.ToLowerInvariant()));

            await queueClient.CreateIfNotExistsAsync(cancellationToken: CancellationToken.None).ConfigureAwait(true);

            var serializedIntegrationEvent = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType());
            await queueClient.SendMessageAsync(serializedIntegrationEvent, CancellationToken.None).ConfigureAwait(true);
        }
    }
}