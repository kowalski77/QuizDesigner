using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using QuizDesigner.Blazor.Server.Support;
using QuizDesigner.Persistence;

namespace QuizDesigner.Blazor.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDatabase<QuizDesignerContext>()
                .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
