namespace mq_receiver_service.Models
{
    public class QueueMessage : IQueueMessage
    {
        public string Text { get; set; }
    }
}
