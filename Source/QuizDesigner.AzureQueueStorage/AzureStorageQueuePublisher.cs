using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Options;
using QuizDesigner.Application.Services;
using QuizDesigner.Events;

namespace QuizDesigner.AzureQueueStorage
{
    public sealed class AzureStorageQueuePublisher : IMessagePublisher
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

            var queueClientOptions = new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            };

            var queueClient = QueueClients.GetOrAdd(integrationEvent.GetType(), type => 
                new QueueClient(this.options.StorageConnectionString, type.Name.ToLowerInvariant(), queueClientOptions));

            await queueClient.CreateIfNotExistsAsync(cancellationToken: CancellationToken.None).ConfigureAwait(true);

            var serializedIntegrationEvent = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType());
            await queueClient.SendMessageAsync(serializedIntegrationEvent, CancellationToken.None).ConfigureAwait(true);
        }
    }
}