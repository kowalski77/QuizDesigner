using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using QuizDesigner.Events;

namespace QuizDesigner.Application.Services
{
    public interface IChannelService
    {
        Task AddAsync(IIntegrationEvent model, CancellationToken cancellationToken);

        IAsyncEnumerable<IIntegrationEvent> GetAsync(CancellationToken cancellationToken);
    }
}