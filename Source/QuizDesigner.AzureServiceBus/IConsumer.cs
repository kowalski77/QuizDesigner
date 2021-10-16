using System.Threading.Tasks;

namespace QuizDesigner.AzureServiceBus
{
    public interface IConsumer<in T>
    {
        Task Consume(T message);
    }
}