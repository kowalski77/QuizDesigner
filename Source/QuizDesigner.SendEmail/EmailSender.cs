using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using QuizDesigner.Application.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace QuizDesigner.SendEmail
{
    public sealed class EmailSender : IEmailSender
    {
        private readonly EmailSenderSettings emailSenderSettings;
        private readonly ILogger<EmailSender> logger;
        private readonly SendGridClient sendGridClient;

        public EmailSender(EmailSenderSettings emailSenderSettings, ILogger<EmailSender> logger)
        {
            this.emailSenderSettings = emailSenderSettings ?? throw new ArgumentNullException(nameof(emailSenderSettings));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.sendGridClient = new SendGridClient(emailSenderSettings.ApiKey);
        }

        public async Task SendAsync(EmailContents emailContents)
        {
            if (emailContents == null)
            {
                throw new ArgumentNullException(nameof(emailContents));
            }

            var from = new EmailAddress(this.emailSenderSettings.Sender);
            var to = new EmailAddress(emailContents.To);

            const string subject = "Quiz Topics: new exam result available!";
            var content = $"Hello there. {Environment.NewLine}" +
                          $"Check out the results for exam: {emailContents.Exam} {Environment.NewLine}{emailContents.Summary}";

            var message = MailHelper.CreateSingleEmail(from, to, subject, content, string.Empty);
            var response = await this.sendGridClient.SendEmailAsync(message).ConfigureAwait(false);

            await this.LogResponseAsync(response).ConfigureAwait(false);
        }

        private async Task LogResponseAsync(Response response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Body.ReadAsStringAsync().ConfigureAwait(false);
                this.logger.LogError($"Error sending e-mail -> Status code: {response.StatusCode.ToString()} - Body: {body}");
            }
        }
    }
}