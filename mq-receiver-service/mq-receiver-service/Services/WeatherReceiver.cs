using mq_receiver_service.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace mq_receiver_service.Services
{
    public class WeatherReceiver : BackgroundService
    {
        private readonly ILogger<WeatherReceiver> logger;
        private IModel channel;
        private IConnection connection;

        public WeatherReceiver(ILogger<WeatherReceiver> logs)
        {
            logger = logs;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
            var factory = new ConnectionFactory
            {
                HostName = rabbitHost,
                DispatchConsumersAsync = true,
                UserName = "guest",
                Password = "guest"
            };

            var queueName = "weather";

            logger.LogInformation("Creating Connection");
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            
            logger.LogInformation("Declaring queue");
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += ReceiveData;
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        private Task ReceiveData(object sender, BasicDeliverEventArgs @event)
        {
            logger.LogInformation("Received new message on the queue");
            
            var payload = JsonSerializer.Deserialize<QueueMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));
            logger.LogInformation($"Received: {payload.Text}");
            
            channel.BasicAck(deliveryTag: @event.DeliveryTag, multiple: false);
            logger.LogInformation("Message receipt acknowledged");

            return Task.CompletedTask;
        }

        ~WeatherReceiver()
        {
            channel.Close();
            connection.Close();
            channel.Dispose();
            connection.Dispose();
        }
    }
}
