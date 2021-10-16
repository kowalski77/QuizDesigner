﻿using System;
using Microsoft.Extensions.DependencyInjection;

namespace QuizDesigner.AzureServiceBus
{
    public static class MessageReceiverExtensions
    {
        public static IServiceCollection AddAzureServiceBusReceiver(this IServiceCollection services, Action<ServiceBusOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var options = new ServiceBusOptions();
            configure.Invoke(options);

            services.AddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>(_ => new ServiceBusClientFactory(options.ConnectionString));
            services.AddSingleton(typeof(IConsumer<>), typeof(Consumer<>));

            services.AddSingleton(sp =>
            {
                var messageReceiverEndPoint = new MessageReceiver(sp);
                foreach (var processorRegistration in options.MessageProcessors)
                {
                    messageReceiverEndPoint.AddProcessor(processorRegistration.Queue, processorRegistration.Type);
                }

                return messageReceiverEndPoint;
            });

            return services;
        }

        public static IServiceCollection UseAzureServiceBusReceiver(this IServiceCollection services)
        {
            services.AddHostedService<ServiceBusReceiverService>();

            return services;
        }
    }
}