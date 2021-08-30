using System.Threading;
using System.Threading.Tasks;
using QuizDesigner.Application.IntegrationEvents;

namespace QuizDesigner.Application.Services.Outbox
{
    public interface IOutboxService
    {
        Task PublishIntegrationEventAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);

        Task PublishPendingIntegrationEventsAsync(CancellationToken cancellationToken = default);
    }
}