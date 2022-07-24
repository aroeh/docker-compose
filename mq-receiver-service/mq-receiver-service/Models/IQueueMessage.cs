namespace mq_receiver_service.Models
{
    public interface IQueueMessage
    {
        string? Id { get; set; }

        string? Text { get; set; }

        DateTime? Received { get; set; }
    }
}
