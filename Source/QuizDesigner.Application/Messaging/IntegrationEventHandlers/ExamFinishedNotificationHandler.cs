using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace QuizDesigner.Application.Messaging.IntegrationEventHandlers
{
    public sealed class ExamFinishedNotificationHandler : INotificationHandler<ExamFinishedNotification>
    {
        private readonly ILogger<ExamFinishedNotificationHandler> logger;

        public ExamFinishedNotificationHandler(ILogger<ExamFinishedNotificationHandler> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(ExamFinishedNotification notification, CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"{notification}");

            return Task.CompletedTask;
        }
    }
}