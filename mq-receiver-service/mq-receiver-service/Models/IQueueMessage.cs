namespace mq_receiver_service.Models
{
    public interface IQueueMessage
    {
        string Text { get; set; }
    }
}
