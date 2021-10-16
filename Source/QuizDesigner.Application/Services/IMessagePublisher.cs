using System.Threading;
using System.Threading.Tasks;
using QuizDesigner.Events;

namespace QuizDesigner.Application.Services
{
    public interface IMessagePublisher
    {
        Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
    }
}