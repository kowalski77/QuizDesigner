using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace QuizDesigner.OutboxSender
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostBuilder, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.UsingRabbitMq();
                    });
                    services.AddMassTransitHostedService();

                    services.AddHostedService(sp => 
                        new OutboxSenderHostedService(
                            new OutboxData(hostBuilder.Configuration.GetConnectionString("DefaultConnection")),
                            sp.GetRequiredService<IPublishEndpoint>(),
                            sp.GetRequiredService<ILogger<OutboxSenderHostedService>>()));
                });
    }
}
