using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuizDesigner.Application.Messaging;
using QuizDesigner.Events;
using QuizDesigner.Outbox;

namespace QuizDesigner.Application.Services
{
    public class OutboxPublisherHostedService : BackgroundService
    {
        private readonly IChannelService channelService;
        private readonly ILogger<OutboxPublisherHostedService> logger;
        private readonly IMessagePublisher messagePublisher;
        private readonly IOutboxDataService outboxDataService;

        public OutboxPublisherHostedService(
            IChannelService channelService, 
            ILogger<OutboxPublisherHostedService> logger, 
            IServiceScopeFactory factory)
        {
            this.channelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (factory == null) throw new ArgumentNullException(nameof(factory));
            this.messagePublisher = factory.CreateScope().ServiceProvider.GetRequiredService<IMessagePublisher>();
            this.outboxDataService = factory.CreateScope().ServiceProvider.GetRequiredService<IOutboxDataService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach (var integrationEvent in this.channelService.GetAsync(stoppingToken))
                {
                    await this.TryPublishAsync(integrationEvent).ConfigureAwait(false);
                }
            }
        }

        private async Task TryPublishAsync(IIntegrationEvent integrationEvent)
        {
            try
            {
                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                cancellationTokenSource.Token.ThrowIfCancellationRequested();

                await this.messagePublisher.PublishAsync(integrationEvent, cancellationTokenSource.Token).ConfigureAwait(true);
            }
            catch (OperationCanceledException e)
            {
                var outboxMessage = OutboxSerializer.Serialize(integrationEvent);
                outboxMessage.Set(EventState.PublishedFailed);

                await this.outboxDataService.SaveMessageAsync(outboxMessage, CancellationToken.None).ConfigureAwait(true);

                this.logger.LogError(e, $"Error publishing message {integrationEvent.Id}, created new outbox message id: {outboxMessage.Id}");
            }
        }
    }
}