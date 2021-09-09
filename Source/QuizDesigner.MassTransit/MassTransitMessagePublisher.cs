using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using QuizCreatedEvents;
using QuizDesigner.Application.Messaging;

namespace QuizDesigner.MassTransit
{
    public class MassTransitMessagePublisher : IMessagePublisher
    {
        private readonly IPublishEndpoint publishEndpoint;

        public MassTransitMessagePublisher(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            if (integrationEvent == null)
            {
                throw new ArgumentNullException(nameof(integrationEvent));
            }

            await this.publishEndpoint.Publish(integrationEvent, integrationEvent.GetType(), cancellationToken).ConfigureAwait(false);
        }
    }
}
