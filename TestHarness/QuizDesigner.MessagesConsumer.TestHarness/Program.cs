using System;
using System.Threading.Tasks;
using MassTransit;

namespace QuizDesigner.MessagesConsumer.TestHarness
{
    internal static class Program
    {
        private static async Task Main()
        {
            await InitializeMassTransit();
        }

        private static async Task InitializeMassTransit()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("quiz-created-listener", e =>
                {
                    e.Consumer<QuizCreatedConsumer>();
                });
            });

            await busControl.StartAsync().ConfigureAwait(false);
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(Console.ReadLine);
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}