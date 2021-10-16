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
        private readonly IExamDataService examDataService;

        public ExamFinishedNotificationHandler(ILogger<ExamFinishedNotificationHandler> logger, IExamDataService examDataService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.examDataService = examDataService ?? throw new ArgumentNullException(nameof(examDataService));
        }

        public async Task Handle(ExamFinishedNotification notification, CancellationToken cancellationToken)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            this.logger.LogInformation($"Notification received of type: {nameof(ExamFinishedNotification)}");

            await this.examDataService.AddAsync(notification, cancellationToken).ConfigureAwait(false);
        }
    }
}