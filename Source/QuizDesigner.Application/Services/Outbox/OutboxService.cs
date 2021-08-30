using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using QuizCreatedEvents;

namespace QuizDesigner.Application.Services.Outbox
{
    public sealed class OutboxService : IOutboxService
    {
        private readonly IOutboxDataService outboxDataService;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ILogger<OutboxService> logger;

        public OutboxService(IOutboxDataService outboxDataService, IPublishEndpoint publishEndpoint, ILogger<OutboxService> logger)
        {
            this.outboxDataService = outboxDataService ?? throw new ArgumentNullException(nameof(outboxDataService));
            this.publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishIntegrationEventAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.publishEndpoint.Publish(integrationEvent, cancellationToken).ConfigureAwait(true);
            }
            catch (Exception e)
            {
                var outboxMessage = OutboxSerializer.Serialize(integrationEvent);
                outboxMessage.Set(EventState.PublishedFailed);

                await this.outboxDataService.SaveMessageAsync(outboxMessage, cancellationToken).ConfigureAwait(true);

                this.logger.LogError(e, $"Error publishing message {integrationEvent.Id}, created new outbox message id: {outboxMessage.Id}");
            }
        }

        public async Task PublishPendingIntegrationEventsAsync(CancellationToken cancellationToken = default)
        {
            var pendingOutboxMessages = await this.outboxDataService.GetNotPublishedAsync(cancellationToken).ConfigureAwait(false);
            if (pendingOutboxMessages.TryGetValue(out var outboxMessages))
            {
                foreach (var outboxMessage in outboxMessages)
                {
                    await this.TryPublishPendingIntegrationEventAsync(outboxMessage, cancellationToken).ConfigureAwait(true);
                }
            }
            else
            {
                this.logger.LogDebug("No outbox messages pending to publish");
            }
        }

        private async Task TryPublishPendingIntegrationEventAsync(
            OutboxMessage outboxMessage, 
            CancellationToken cancellationToken)
        {
            try
            {
                var integrationEvent = OutboxSerializer.Deserialize(outboxMessage);
                await this.publishEndpoint.Publish(integrationEvent, cancellationToken).ConfigureAwait(false);

                outboxMessage.Set(EventState.Published);
                await this.outboxDataService.UpdateMessageAsync(outboxMessage, cancellationToken).ConfigureAwait(true);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Error publishing message, id: {outboxMessage.Id}");
            }
        }
    }
}