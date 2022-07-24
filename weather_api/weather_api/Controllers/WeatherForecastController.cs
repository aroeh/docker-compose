using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using StackExchange.Redis;
using System.Text;
using weather_api.Models;
using weather_api.Services;

namespace weather_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IConnectionMultiplexer cache;
        private readonly IDatabase cacheDb;
        private readonly IRabbitMQService rabbitMQ;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConnectionMultiplexer cacheCon, IRabbitMQService rabbit)
        {
            _logger = logger;
            cache = cacheCon;
            cacheDb = cache.GetDatabase();

            rabbitMQ = rabbit;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            _logger.LogInformation("getting weather forecast");

            _logger.LogInformation("checking cache for value");
            var cacheString = cacheDb.StringGet("weather");
            if (!string.IsNullOrWhiteSpace(cacheString))
            {
                _logger.LogInformation("data found in cache.  returning cached value");
                var cacheVal = JsonConvert.DeserializeObject<WeatherForecast[]>(cacheString);
                return Ok(cacheVal);
            }

            _logger.LogInformation("data not found in cache");
            _logger.LogInformation("retrieving data from repository");

            var weather = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            _logger.LogInformation("data found...adding to cache");
            cacheDb.StringSet("weather", JsonConvert.SerializeObject(weather));

            return Ok(weather);
        }

        [HttpPost(Name = "PublishDay")]
        public async Task<IActionResult> Post([FromBody] string message)
        {
            _logger.LogInformation("new message received");

            _logger.LogInformation("publishing message to queue");
            rabbitMQ.Publish(message);

            return Ok(message);
        }
    }
}