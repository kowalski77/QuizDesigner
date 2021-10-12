using System.Threading;
using System.Threading.Tasks;
using QuizDesigner.Events;

namespace QuizDesigner.Application.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
    }
}