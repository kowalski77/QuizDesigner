using Microsoft.Extensions.DependencyInjection;
using QuizDesigner.Application.Messaging;

namespace QuizDesigner.AzureQueueStorage
{
    public static class AzureQueueStorageExtensions
    {
        public static IServiceCollection AddAzureQueueStorage(this IServiceCollection services)
        {
            services.AddScoped<IMessagePublisher, AzureStorageQueuePublisher>();

            return services;
        }
    }
}