using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arch.Utils.Functional.Monads;

namespace QuizDesigner.Application.Services.Outbox
{
    public interface IOutboxDataService
    {
        Task SaveMessageAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default);

        Task UpdateMessageAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default);

        Task<Maybe<IReadOnlyList<OutboxMessage>>> GetNotPublishedAsync(CancellationToken cancellationToken = default);
    }
}