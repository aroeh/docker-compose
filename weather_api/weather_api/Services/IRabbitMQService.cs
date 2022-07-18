namespace weather_api.Services
{
    public interface IRabbitMQService
    {
        bool Publish(string message);
    }
}
