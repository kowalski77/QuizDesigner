using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuizCreatedEvents;

namespace QuizDesigner.Application.Services.Outbox
{
    public class OutboxSenderHostedService : BackgroundService
    {
        private readonly ChannelService<QuizCreated> channelService;
        private readonly ILogger<OutboxSenderHostedService> logger;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IOutboxDataService outboxDataService;

        public OutboxSenderHostedService(
            ChannelService<QuizCreated> channelService, 
            ILogger<OutboxSenderHostedService> logger, 
            IServiceScopeFactory factory)
        {
            this.channelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (factory == null) throw new ArgumentNullException(nameof(factory));
            this.publishEndpoint = factory.CreateScope().ServiceProvider.GetRequiredService<IPublishEndpoint>();
            this.outboxDataService = factory.CreateScope().ServiceProvider.GetRequiredService<IOutboxDataService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach (var quizCreated in this.channelService.Get(stoppingToken))
                {
                    await this.TryPublishAsync(quizCreated).ConfigureAwait(false);
                }
            }
        }

        private async Task TryPublishAsync(QuizCreated quizCreated)
        {
            try
            {
                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                cancellationTokenSource.Token.ThrowIfCancellationRequested();

                await this.publishEndpoint.Publish(quizCreated, cancellationTokenSource.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException e)
            {
                var outboxMessage = OutboxSerializer.Serialize(quizCreated);
                outboxMessage.Set(EventState.PublishedFailed);

                await this.outboxDataService.SaveMessageAsync(outboxMessage, CancellationToken.None).ConfigureAwait(true);

                this.logger.LogError(e, $"Error publishing message {quizCreated.Id}, created new outbox message id: {outboxMessage.Id}");
            }
        }
    }
}