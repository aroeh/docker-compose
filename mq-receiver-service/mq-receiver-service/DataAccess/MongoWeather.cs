using MongoDB.Driver;
using mq_receiver_service.Models;

namespace mq_receiver_service.DataAccess
{
    public class MongoWeather : IMongoWeather
    {
        private readonly ILogger<MongoWeather> logger;
        private readonly IMongoClient mongoClient;
        private readonly IMongoDatabase mongoDb;
        private readonly IMongoCollection<QueueMessage> collection;

        public MongoWeather(ILogger<MongoWeather> logs)
        {
            logger = logs;

            logger.LogInformation("Connecting to Mongo Client");
            mongoClient = new MongoClient(Environment.GetEnvironmentVariable("MONGODB_CONNSTRING"));

            logger.LogInformation("Getting database");
            mongoDb = mongoClient.GetDatabase("weather");

            logger.LogInformation("Getting collection");
            collection = mongoDb.GetCollection<QueueMessage>("new_messages");
        }

        /// <summary>
        /// Adds a single new message to the mongodb collection
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<string> InsertMessage(QueueMessage message)
        {
            logger.LogInformation("Inserting new document");
            await collection.InsertOneAsync(message);
            return message.Id;
        }

        /// <summary>
        /// Gets all weather messages from the mongodb collection
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<QueueMessage>> GetMessages()
        {
            logger.LogInformation("Getting documents");
            return await collection.Find(c => true).ToListAsync();
        }
    }
}
