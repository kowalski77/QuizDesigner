using Microsoft.Extensions.DependencyInjection;
using QuizDesigner.Application.Messaging;

namespace QuizDesigner.AzureServiceBus
{
    public static class AzureServiceBusExtensions
    {
        public static IServiceCollection AddAzureServiceBus(this IServiceCollection services)
        {
            services.AddScoped<IMessagePublisher, AzureServiceBusPublisher>();

            return services;
        }
    }
}