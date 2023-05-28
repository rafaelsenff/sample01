using RabbitMQ.Client;
using System.Text;

namespace BaseRabbitMq
{
    public class RabbitEventPublisher : IRabbitEventPublisher
    {
        public async Task<bool> SendEvent<T>(T evento) where T : BaseEvent
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq", Port = 5672 };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var eventName = evento.GetType().Name;

            channel.QueueDeclare(queue: eventName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var message = Newtonsoft.Json.JsonConvert.SerializeObject(evento);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: eventName,
                                 basicProperties: null,
                                 body: body);

            return true;
        }
    }
}