using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;

namespace BaseRabbitMq
{
    public class RabbitMqConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqConsumerService(IServiceProvider serviceProvider, ConnectionFactory connectionFactory)
        {
            _serviceProvider = serviceProvider;
            _connectionFactory = connectionFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Wait until the application host is fully started
            await Task.Delay(10000, stoppingToken);

            // Initialize RabbitMQ connection and consume events
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            InitializeQueues(_channel);
            InitializeEventHandlers(_channel);

            while (!stoppingToken.IsCancellationRequested)
            {
                // Keep the service running
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            // Close the RabbitMQ connection and channel
            _channel.Close();
            _connection.Close();

            await base.StopAsync(stoppingToken);
        }

        private void InitializeQueues(IModel channel)
        {
            // Declare the queues for each event type
            var eventTypes = Assembly.GetEntryAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
                .ToList();

            foreach (var eventType in eventTypes)
            {
                var queueName = eventType.Name.Replace("Handler",string.Empty);
                channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            }
        }

        private void InitializeEventHandlers(IModel channel)
        {
            var eventHandlerTypes = Assembly.GetEntryAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
                .ToList();

            foreach (var eventHandlerType in eventHandlerTypes)
            {
                var eventType = eventHandlerType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                    .Select(i => i.GetGenericArguments()[0])
                    .FirstOrDefault();

                if (eventType != null)
                {
                    var queueName = eventType.Name;
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, args) =>
                    {
                        var body = args.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var eventData = Newtonsoft.Json.JsonConvert.DeserializeObject(message, eventType);

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var eventHandler = scope.ServiceProvider.GetService(eventHandlerType);
                            var handleMethod = eventHandlerType.GetMethod("Handle");
                            handleMethod.Invoke(eventHandler, new[] { eventData });
                        }
                    };
                    channel.BasicConsume(queueName, autoAck: true, consumer);
                }
            }
        }
    }
}