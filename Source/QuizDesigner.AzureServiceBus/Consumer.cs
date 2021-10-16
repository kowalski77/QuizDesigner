using System;
using System.Threading.Tasks;
using MediatR;

namespace QuizDesigner.AzureServiceBus
{
    public sealed class Consumer<T> : IConsumer<T>
    {
        private readonly IMediator mediator;
        private readonly ITranslator<T> translator;

        public Consumer(IMediator mediator, ITranslator<T> translator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.translator = translator ?? throw new ArgumentNullException(nameof(translator));
        }

        public async Task Consume(T message)
        {
            var notification = this.translator.Translate(message);

            await this.mediator.Publish(notification).ConfigureAwait(false);
        }
    }
}