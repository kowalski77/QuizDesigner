using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace QuizDesigner.Application
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });
            services.AddMassTransitHostedService();

            return services;
        }
    }
}