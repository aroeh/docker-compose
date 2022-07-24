namespace weather_api.Models
{
    public class QueueMessage : IQueueMessage
    {
        public string? Id { get; set; }

        public string? Text { get; set; }

        public DateTime? Received { get; set; }
    }
}
