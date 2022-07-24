using mq_receiver_service.Models;

namespace mq_receiver_service.DataAccess
{
    public interface IMongoWeather
    {
        /// <summary>
        /// Adds a single new message to the mongodb collection
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<string> InsertMessage(QueueMessage message);

        /// <summary>
        /// Gets all weather messages from the mongodb collection
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<QueueMessage>> GetMessages();
    }
}
