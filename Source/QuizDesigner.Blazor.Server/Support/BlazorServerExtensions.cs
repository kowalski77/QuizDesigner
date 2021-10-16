using System;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizDesigner.Application.Services;
using QuizDesigner.AzureQueueStorage;
using QuizDesigner.Blazor.App.Services;

namespace QuizDesigner.Blazor.Server.Support
{
    public static class BlazorServerExtensions
    {
        public static void ConfigureApplicationUser(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var secretClient = ConfigureAzureKeyVault(sp.GetRequiredService<IConfiguration>());
                var applicationUser = new ApplicationUser
                {
                    Email = secretClient.GetSecret("app-user-email").Value.Value,
                    Password = secretClient.GetSecret("app-user-password").Value.Value
                };

                return applicationUser;
            });
        }

        public static void AddAzureQueueStorage(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.Configure<AzureQueueStorageOptions>(configuration.GetSection(nameof(AzureQueueStorageOptions)));
            services.AddScoped<IMessagePublisher, AzureStorageQueuePublisher>();
        }

        private static SecretClient ConfigureAzureKeyVault(IConfiguration configuration)
        {
            var options = new SecretClientOptions
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };

            var client = new SecretClient(new Uri(configuration.GetValue<string>("AzureKeyVaultUrl")), new DefaultAzureCredential(), options);

            return client;
        }
    }
}