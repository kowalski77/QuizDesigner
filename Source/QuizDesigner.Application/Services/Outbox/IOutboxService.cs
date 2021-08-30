using System.Threading;
using System.Threading.Tasks;
using QuizCreatedEvents;

namespace QuizDesigner.Application.Services.Outbox
{
    public interface IOutboxService
    {
        Task PublishIntegrationEventAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
            where T : IIntegrationEvent;

        Task PublishPendingIntegrationEventsAsync(CancellationToken cancellationToken = default);
    }
}