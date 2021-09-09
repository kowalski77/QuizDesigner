using Microsoft.Extensions.DependencyInjection;
using QuizDesigner.Application.Services;

namespace QuizDesigner.Application
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IQuizService, QuizService>();
            services.AddSingleton<IChannelService, ChannelService>();

            return services;
        }
    }
}