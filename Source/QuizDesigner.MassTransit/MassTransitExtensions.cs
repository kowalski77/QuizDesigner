using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using QuizDesigner.Application.Messaging;

namespace QuizDesigner.MassTransit
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });
            services.AddMassTransitHostedService();

            services.AddScoped<IMessagePublisher, MassTransitMessagePublisher>();

            return services;
        }
    }
}