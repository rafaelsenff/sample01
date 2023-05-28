using System.Reflection;
using Microsoft.AspNetCore.Builder;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;

namespace BaseRabbitMq
{
    public static class RabbitMqExtensions
    {
        public static void UseRabbitMqLib(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IRabbitEventPublisher, RabbitEventPublisher>();

            builder.Services.AddSingleton<ConnectionFactory>(provider =>
            {
                // Configure the connection details here
                var factory = new ConnectionFactory { HostName = "rabbitmq", Port = 5672 };
                return factory;
            });

            builder.Services.AddScoped<IModel>(provider =>
            {
                var connectionFactory = provider.GetRequiredService<ConnectionFactory>();
                var connection = connectionFactory.CreateConnection();
                var channel = connection.CreateModel();
                return channel;
            });

            var eventHandlerTypes = Assembly.GetEntryAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)));

            foreach (var eventHandlerType in eventHandlerTypes)
            {
                builder.Services.AddScoped(eventHandlerType);
            }

            builder.Services.AddHostedService<RabbitMqConsumerService>();
        }
    }
}