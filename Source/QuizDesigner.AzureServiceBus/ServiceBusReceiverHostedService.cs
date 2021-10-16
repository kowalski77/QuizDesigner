using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace QuizDesigner.AzureServiceBus
{
    public class ServiceBusReceiverHostedService : IHostedService
    {
        private readonly MessageReceiver messageReceiver;

        public ServiceBusReceiverHostedService(MessageReceiver messageReceiver)
        {
            this.messageReceiver = messageReceiver ?? throw new ArgumentNullException(nameof(messageReceiver));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.messageReceiver.StartAsync().ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await this.messageReceiver.StopAsync().ConfigureAwait(false);
        }
    }
}