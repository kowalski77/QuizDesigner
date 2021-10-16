using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace QuizDesigner.AzureServiceBus
{
    public class ServiceBusReceiverService : IHostedService
    {
        private readonly MessageReceiver messageReceiver;

        public ServiceBusReceiverService(MessageReceiver messageReceiver)
        {
            this.messageReceiver = messageReceiver ?? throw new ArgumentNullException(nameof(messageReceiver));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.messageReceiver.StartAsync().ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}