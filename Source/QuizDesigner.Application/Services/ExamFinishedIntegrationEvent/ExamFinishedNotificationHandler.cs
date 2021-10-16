using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace QuizDesigner.Application.Services.ExamFinishedIntegrationEvent
{
    public sealed class ExamFinishedNotificationHandler : INotificationHandler<ExamFinishedNotification>
    {
        private readonly ILogger<ExamFinishedNotificationHandler> logger;
        private readonly IExamDataService examDataService;
        private readonly IQuizDataProvider quizDataProvider;
        private readonly IEmailSender emailSender;

        public ExamFinishedNotificationHandler(
            ILogger<ExamFinishedNotificationHandler> logger,
            IExamDataService examDataService,
            IQuizDataProvider quizDataProvider,
            IEmailSender emailSender)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.examDataService = examDataService ?? throw new ArgumentNullException(nameof(examDataService));
            this.quizDataProvider = quizDataProvider ?? throw new ArgumentNullException(nameof(quizDataProvider));
            this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task Handle(ExamFinishedNotification notification, CancellationToken cancellationToken)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            this.logger.LogInformation($"Notification received of type: {nameof(ExamFinishedNotification)}");

            var exam = await this.examDataService.AddAsync(MapExam(notification), cancellationToken).ConfigureAwait(false);

            await this.SendEmailAsync(notification.QuizId, exam, cancellationToken).ConfigureAwait(false);
        }

        private async Task SendEmailAsync(Guid quizId, Exam exam, CancellationToken cancellationToken)
        {
            var quiz = await this.quizDataProvider.GetAsync(quizId, cancellationToken).ConfigureAwait(false);

            var emailOptions = new EmailOptions($"Exam result for: {exam.Summary.Candidate}", "c.aranda.diaz@avanade.com", exam.Summary.ToString());
            await this.emailSender.SendAsync(emailOptions).ConfigureAwait(false);
        }

        private static Exam MapExam(ExamFinishedNotification notification)
        {
            var exam = new Exam(
                notification.Id,
                new Summary(
                    notification.QuizId, notification.Passed, notification.Candidate,
                    notification.CorrectQuestionsCollection, notification.WrongQuestionsCollection));

            return exam;
        }
    }
}