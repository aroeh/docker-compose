namespace weather_api.Models
{
    public class QueueMessage : IQueueMessage
    {
        public string Text { get; set; }
    }
}
