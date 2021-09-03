using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using QuizDesigner.Application.Services;
using QuizDesigner.Application.Services.Outbox;

namespace QuizDesigner.Application
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IQuizService, QuizService>();
            services.AddSingleton<IChannelService, ChannelService>();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });
            services.AddMassTransitHostedService();

            return services;
        }
    }
}