using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using QuizCreatedEvents;

namespace QuizDesigner.Application.Services.Outbox
{
    public interface IChannelService
    {
        Task Add(IIntegrationEvent model, CancellationToken cancellationToken);
        IAsyncEnumerable<IIntegrationEvent> Get(CancellationToken cancellationToken);
    }
}