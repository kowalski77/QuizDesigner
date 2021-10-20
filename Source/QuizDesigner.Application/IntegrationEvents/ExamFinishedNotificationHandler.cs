using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using QuizDesigner.Application.Services;

namespace QuizDesigner.Application.IntegrationEvents
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

            await this.SendEmailAsync(notification, exam, cancellationToken).ConfigureAwait(false);
        }

        private async Task SendEmailAsync(ExamFinishedNotification notification, Exam exam, CancellationToken cancellationToken)
        {
            var quiz = await this.quizDataProvider.GetAsync(notification.QuizId, cancellationToken).ConfigureAwait(false);

            var emailContents = new EmailContents(quiz.Email, quiz.Name, exam.Summary.ToString());
            await this.emailSender.SendAsync(emailContents).ConfigureAwait(false);
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