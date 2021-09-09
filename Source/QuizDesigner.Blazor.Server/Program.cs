using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizDesigner.Application.Services;
using QuizDesigner.Blazor.Server.Support;
using QuizDesigner.Persistence;
using QuizDesigner.Persistence.Outbox;

namespace QuizDesigner.Blazor.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDatabase<OutboxDbContext>()
                .MigrateDatabase<QuizDesignerContext>()
                .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<OutboxPublisherHostedService>();
                });
    }
}
