﻿using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizDesigner.Application;

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
        }
    }
}