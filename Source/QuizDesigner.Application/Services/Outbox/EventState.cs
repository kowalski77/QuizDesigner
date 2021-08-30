namespace QuizDesigner.Application.Services.Outbox
{
    public enum EventState
    {
        Unknown = 0,
        Published = 1,
        PublishedFailed = 2
    }
}