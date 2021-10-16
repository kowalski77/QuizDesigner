namespace QuizDesigner.SendEmail
{
    public class EmailSenderSettings
    {
        public string ApiKey { get; set; } = string.Empty;

        public string Sender { get; set; } = string.Empty;

        public string PlainTextContent { get; set; } = string.Empty;

        public string HtmlContent { get; set; } = string.Empty;
    }
}