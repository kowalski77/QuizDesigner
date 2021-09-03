using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizDesigner.Application;
using QuizDesigner.Outbox;
using QuizDesigner.Persistence.Outbox;

namespace QuizDesigner.Persistence
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddEntityFramework(connectionString);

            services.AddScoped<IQuestionsDataService, QuestionsDataService>();
            services.AddScoped<IQuestionsDataProvider, QuestionsDataProvider>();
            services.AddScoped<IQuizDataService, QuizDataService>();
            services.AddScoped<IQuizDataProvider, QuizDataProvider>();
            services.AddScoped<IOutboxDataService, OutboxDataService>();

            return services;
        }

        private static void AddEntityFramework(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContextFactory<QuizDesignerContext>(options =>
            {
                options.UseSqlServer(connectionString,
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(QuizDesignerContext).GetTypeInfo().Assembly.GetName().Name);
                            sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
                        })
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });

            services.AddDbContextFactory<OutboxDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(OutboxDbContext).GetTypeInfo().Assembly.GetName().Name);
                            sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
                        })
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });
        }
    }
}