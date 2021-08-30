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
            services.AddScoped<IOutboxService, OutboxService>();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });
            services.AddMassTransitHostedService();

            return services;
        }
    }
}