using System;
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
        private readonly AzureQueueStorageOptions options;

        public AzureStorageQueuePublisher(IOptions<AzureQueueStorageOptions> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Azure Queue naming conventions")]
        public async Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            if (integrationEvent == null)
            {
                throw new ArgumentNullException(nameof(integrationEvent));
            }

            // TODO: reuse client
            var queueClient = new QueueClient(this.options.StorageConnectionString, integrationEvent.GetType().Name.ToLowerInvariant());
            await queueClient.CreateIfNotExistsAsync(cancellationToken: CancellationToken.None).ConfigureAwait(true);

            var serializedIntegrationEvent = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType());
            var response = await queueClient.SendMessageAsync(serializedIntegrationEvent, CancellationToken.None).ConfigureAwait(true);
            
        }
    }
}
