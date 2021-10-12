using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using QuizDesigner.Events;

namespace QuizDesigner.Application.Services
{
    public sealed class ChannelService : IChannelService
    {
        private readonly Channel<IIntegrationEvent> serviceChannel;

        public ChannelService()
        {
            this.serviceChannel = Channel.CreateBounded<IIntegrationEvent>(new BoundedChannelOptions(50)
            {
                SingleReader = true,
                SingleWriter = false,
                FullMode = BoundedChannelFullMode.DropWrite
            });
        }

        public async Task AddAsync(IIntegrationEvent model, CancellationToken cancellationToken)
        {
            await this.serviceChannel.Writer.WriteAsync(model, cancellationToken).ConfigureAwait(true);
        }

        public IAsyncEnumerable<IIntegrationEvent> GetAsync(CancellationToken cancellationToken)
        {
            return this.serviceChannel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}