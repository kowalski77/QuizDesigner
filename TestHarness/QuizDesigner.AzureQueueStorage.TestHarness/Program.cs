using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace QuizDesigner.AzureQueueStorage.TestHarness
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddHostedService<MessageDequeueHostedService>();
                })
                .ConfigureAppConfiguration(cfg =>
                {
                    cfg.AddUserSecrets<Program>();
                });
        }
    }
}