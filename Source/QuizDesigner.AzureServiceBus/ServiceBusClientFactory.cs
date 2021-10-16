using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace QuizDesigner.AzureServiceBus
{
    public class ServiceBusClientFactory : IServiceBusClientFactory, IAsyncDisposable
    {
        public ServiceBusClientFactory(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("The Azure Service Bus connection string cannot be empty");
            }

            this.Client = new ServiceBusClient(connectionString);
        }

        public ServiceBusClient Client { get; }

        public async ValueTask DisposeAsync()
        {
            await this.Client.DisposeAsync().ConfigureAwait(false);
        }
    }
}