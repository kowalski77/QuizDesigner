using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuizDesigner.Events;
using QuizDesigner.Outbox;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.OutboxSender
{
    public class OutboxSenderHostedService : BackgroundService
    {
        private readonly OutboxData outboxData;
        private readonly IMessagePublisher messagePublisher;
        private readonly ILogger<OutboxSenderHostedService> logger;

        public OutboxSenderHostedService(
            OutboxData outboxData, 
            IMessagePublisher messagePublisher, 
            ILogger<OutboxSenderHostedService> logger)
        {
            this.outboxData = outboxData ?? throw new ArgumentNullException(nameof(outboxData));
            this.messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this.logger.LogDebug("Checking for pending outbox messages...");

                var outboxMessageCollection = await this.outboxData.GetNotPublishedAsync().ConfigureAwait(false);

                if(outboxMessageCollection.Count == 0)
                {
                    this.logger.LogDebug("No pending outbox messages to publish.");
                }

                foreach (var outboxMessage in outboxMessageCollection)
                {
                    await this.TryPublishAsync(outboxMessage).ConfigureAwait(false);
                }

                await Task.Delay(60 * 1000, stoppingToken).ConfigureAwait(false);
            }
        }

        private async Task TryPublishAsync(OutboxMessage outboxMessage)
        {
            var integrationEvent = (IIntegrationEvent)OutboxSerializer.Deserialize(outboxMessage, typeof(QuizCreated).Assembly);
            try
            {
                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                cancellationTokenSource.Token.ThrowIfCancellationRequested();

                await this.messagePublisher.PublishAsync(integrationEvent, cancellationTokenSource.Token).ConfigureAwait(false);

                this.logger.LogInformation($"Outbox message with id:{outboxMessage.Id} successfully published");

                await this.outboxData.SetMessageAsPublishedAsync(outboxMessage.Id).ConfigureAwait(false);
            }
            catch (OperationCanceledException e)
            {
                this.logger.LogError(e, $"Error re-publishing outbox message with id: {outboxMessage.Id}");
            }
        }
    }
}