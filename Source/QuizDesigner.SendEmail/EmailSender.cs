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

        public async Task SendAsync(EmailOptions emailOptions)
        {
            if (emailOptions == null)
            {
                throw new ArgumentNullException(nameof(emailOptions));
            }

            var from = new EmailAddress(this.emailSenderSettings.Sender);
            var to = new EmailAddress(emailOptions.To);

            var message = MailHelper.CreateSingleEmail(from, to, emailOptions.Subject, this.emailSenderSettings.PlainTextContent, this.emailSenderSettings.HtmlContent);
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