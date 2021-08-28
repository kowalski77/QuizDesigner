using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace QuizDesigner.Services
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
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