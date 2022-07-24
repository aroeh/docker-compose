using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mq_receiver_service.Models
{
    public class QueueMessage : IQueueMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("text")]
        public string? Text { get; set; }

        [BsonElement("received")]
        public DateTime? Received { get; set; }
    }
}
