using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Outbox
{
    public interface IOutboxDataService
    {
        Task SaveMessageAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default);
    }
}