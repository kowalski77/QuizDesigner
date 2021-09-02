using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace QuizDesigner.Application.Services.Outbox
{
    public sealed class ChannelService<TMessage> where TMessage : class
    {
        private readonly Channel<TMessage> serviceChannel;

        public ChannelService()
        {
            this.serviceChannel = Channel.CreateBounded<TMessage>(new BoundedChannelOptions(50)
            {
                SingleReader = true,
                SingleWriter = false,
                FullMode = BoundedChannelFullMode.DropWrite
            });
        }

        public async Task Add(TMessage model, CancellationToken cancellationToken)
        {
            await this.serviceChannel.Writer.WriteAsync(model, cancellationToken).ConfigureAwait(true);
        }

        public IAsyncEnumerable<TMessage> Get(CancellationToken cancellationToken)
        {
            return this.serviceChannel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}