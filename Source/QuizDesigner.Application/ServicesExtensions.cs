using MediatR;
using Microsoft.Extensions.DependencyInjection;
using QuizDesigner.Application.Services;
using QuizDesigner.Application.Services.ExamFinishedIntegrationEvent;
using QuizDesigner.AzureServiceBus;
using QuizDesigner.Events;

namespace QuizDesigner.Application
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IQuizService, QuizService>();
            services.AddSingleton<IChannelService, ChannelService>();

            services.AddMediatR(typeof(ExamFinishedNotification).Assembly);
            services.AddTranslator<ExamFinished, ExamFinishedTranslator>();

            return services;
        }
    }
}