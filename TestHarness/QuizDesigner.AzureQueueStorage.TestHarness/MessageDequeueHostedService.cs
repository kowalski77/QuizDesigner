using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using QuizDesigner.Events;

namespace QuizDesigner.AzureQueueStorage.TestHarness
{
    public class MessageDequeueHostedService : BackgroundService
    {
        private readonly QueueClient queueClient;

        public MessageDequeueHostedService(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("StorageConnectionString");
            this.queueClient = new QueueClient(connectionString, nameof(QuizCreated).ToLowerInvariant());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (await this.queueClient.ExistsAsync(stoppingToken))
                {
                    QueueMessage[] receivedMessages = await this.queueClient.ReceiveMessagesAsync(5, TimeSpan.FromMinutes(1), stoppingToken);
                    foreach (var message in receivedMessages)
                    {
                        Console.WriteLine($"De-queued message: '{message.Body}'");

                        await this.queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt, stoppingToken);
                    }
                }

                await Task.Delay(10000, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}