using MediatR;

namespace QuizDesigner.AzureServiceBus
{
    public interface ITranslator<in T>
    {
        INotification Translate(T message);
    }
}