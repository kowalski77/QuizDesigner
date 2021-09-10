using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace QuizDesigner.OutboxSender
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
                .ConfigureServices((hostBuilder, services) =>
                {
                    var storageConnectionString = hostBuilder.Configuration.GetValue<string>("StorageConnectionString");
                    services.AddScoped<IMessagePublisher>(_ => new AzureStorageQueuePublisher(storageConnectionString));

                    var sqlConnectionString = hostBuilder.Configuration.GetConnectionString("DefaultConnection");
                    services.AddScoped(_ => new OutboxData(sqlConnectionString));

                    services.AddHostedService<OutboxSenderHostedService>();
                })
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddUserSecrets<Program>();
                });
        }
    }
}