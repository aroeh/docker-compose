using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using weather_api.Models;

namespace weather_api.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ILogger<RabbitMQService> logger;

        private readonly ConnectionFactory rabbitFactory;
        private readonly IConnection rabbitConnection;
        private readonly IModel rabbitChannel;
        private readonly string queue;

        public RabbitMQService(ILogger<RabbitMQService> logs)
        {
            logger = logs;

            queue = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE");
            var rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
            rabbitFactory = new ConnectionFactory
            {
                HostName = rabbitHost
            };
            rabbitConnection = rabbitFactory.CreateConnection();
            rabbitChannel = rabbitConnection.CreateModel();
            rabbitChannel.QueueDeclare(queue, false, false, false);
        }

        public bool Publish(string message)
        {
            var queueMessage = new QueueMessage
            {
                Text = message
            };

            logger.LogInformation("publishing message to queue");
            var msgBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(queueMessage));
            rabbitChannel.BasicPublish("", queue, null, msgBody);

            return true;
        }
    }
}
