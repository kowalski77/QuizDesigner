using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Options;
using QuizCreatedEvents;
using QuizDesigner.Application.Messaging;

namespace QuizDesigner.AzureQueueStorage
{
    public class AzureStorageQueuePublisher : IMessagePublisher
    {
        private static readonly ConcurrentDictionary<Type, QueueClient> QueueClients = new();

        private readonly AzureQueueStorageOptions options;

        public AzureStorageQueuePublisher(IOptions<AzureQueueStorageOptions> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Azure Queue naming conventions")]
        public async Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            if (integrationEvent == null)
            {
                throw new ArgumentNullException(nameof(integrationEvent));
            }

            var queueClient = QueueClients.GetOrAdd(integrationEvent.GetType(), type => new QueueClient(this.options.StorageConnectionString, type.Name.ToLowerInvariant()));

            await queueClient.CreateIfNotExistsAsync(cancellationToken: CancellationToken.None).ConfigureAwait(true);

            var serializedIntegrationEvent = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType());
            await queueClient.SendMessageAsync(serializedIntegrationEvent, CancellationToken.None).ConfigureAwait(true);
        }
    }
}